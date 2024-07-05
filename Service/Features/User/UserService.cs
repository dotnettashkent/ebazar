using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Service.Data;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.CommandR;
using Stl.Fusion;
using Stl.Fusion.EntityFramework;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reactive;
using System.Security.Claims;
using System.Text;

namespace Service.Features.User
{
    public class UserService : IUserService
    {
        #region Initialize

        private readonly DbHub<AppDbContext> dbHub;
        private readonly IProductService productService;
        private readonly IConfiguration configuration;
        private readonly ICommander commander;
        public UserService(DbHub<AppDbContext> dbHub, IProductService productService, IConfiguration configuration, ICommander commander)
        {
            this.dbHub = dbHub;
            this.productService = productService;
            this.configuration = configuration;
            this.commander = commander;
        }
        #endregion

        #region Queries

        public async Task<TableResponse<OrderResultView>> GetUserOrdersByProcessAsync(TableOptions options, CancellationToken cancellationToken = default)
        {
            var isValid = ValidateToken(options.token!);
            var isUser = IsUser(isValid);
            await Invalidate();
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);

            var orders = dbContext.Orders.AsQueryable().Where(x => x.Status == options.status && x.UserId == isUser.Id);

            Sorting(ref orders, options);

            var count = await orders.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
            var items = await orders.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
            decimal totalPage = (decimal)count / (decimal)options.page_size;

            var itemsResult = new List<OrderResultView>();

            foreach (var item in items)
            {
                OrderResultView temp = new()
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    City = item.City,
                    Street = item.Street,
                    Region = item.Region,
                    HomeNumber = item.HomeNumber,
                    CommentForCourier = item.CommentForCourier,
                    DeliveryTime = item.DeliveryTime,
                    PaymentType = item.PaymentType,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    ExtraPhoneNumber = item.ExtraPhoneNumber,
                    Status = item.Status,
                    Products = Serialize(item.Products!)
                };

                itemsResult.Add(temp);
            }

            return new TableResponse<OrderResultView>()
            {
                Items = itemsResult,
                TotalItems = count,
                AllPage = (int)Math.Ceiling(totalPage),
                CurrentPage = options.page
            };
        }

        

        public async virtual Task<TableResponse<UserView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
        {
            var isValid = ValidateToken(options.token);
            if (!IsAdminUser(isValid))
            {
                throw new CustomException("User does not have permission");
            }
            await Invalidate();
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var users = from s in dbContext.UsersEntities select s;

            if (!String.IsNullOrEmpty(options.search))
            {
                var search = options.search.ToLower();
                users = users.Where(s =>
                         s.PhoneNumber != null && s.PhoneNumber.ToLower().Contains(search)
                );
            }

            Sorting(ref users, options);

            var count = await users.AsNoTracking().CountAsync();
            var items = await users.AsNoTracking().Paginate(options).ToListAsync();
            decimal totalPage = (decimal)count / (decimal)options.page_size;
            return new TableResponse<UserView>()
            {
                Items = items.MapToViewList(),
                TotalItems = count,
                AllPage = (int)Math.Ceiling(totalPage),
                CurrentPage = options.page
            };
        }

        public async virtual Task<UserView> GetById(long id, CancellationToken cancellationToken = default)
        {
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var user = await dbContext.UsersEntities
                .Include(x => x.Cart)
                .Include(x => x.Orders)
                .Include(x => x.Addresses)
                .Include(x => x.Favourite)
                .FirstOrDefaultAsync(x => x.Id == id);

            return user == null ? throw new CustomException("User was not found") : user.MapToView();
        }

        public async virtual Task<UserResultView> Get(long Id, CancellationToken cancellationToken = default)
        {
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var user = await dbContext.UsersEntities
                .FirstOrDefaultAsync(x => x.Id == Id);

            return user == null ? throw new CustomException("User was not found") : user.MapToResultView();
        }

        public async virtual Task<UserView> GetByToken(string token, CancellationToken cancellationToken)
        {
            var secretKey = configuration.GetSection("JwtSettings:SecretKey").Value;

            var phoneNumber = GetPhoneNumber(token);

            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);

            var user = await dbContext.UsersEntities
                .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken);

            return user == null ? throw new CustomException("User was not found") : user.MapToView();
        }

        private string GetPhoneNumber(string token)
        {
            var jwtEncodedString = token.Substring(7);

            var secondToken = new JwtSecurityToken(jwtEncodedString);
            var json = secondToken.Payload.Values.FirstOrDefault();

            if (json == null)
                throw new CustomException("Payload is null");
            else
            {
                return json?.ToString() ?? string.Empty;
            }
        }
        #endregion

        #region Mutations
        public async virtual Task<bool> Create(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return false;
            }

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var exists = await dbContext.UsersEntities.FirstOrDefaultAsync(x => x.PhoneNumber == command.Entity.PhoneNumber);

            if (exists == null)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Entity.Password);
                command.Entity.Password = passwordHash;
                UserEntity user = new UserEntity();
                Reattach(user, command.Entity, dbContext);

                dbContext.Update(user);
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            else
            {
                throw new CustomException("User already exists");
            }
        }



        public async virtual Task Delete(DeleteUserCommand command, CancellationToken cancellationToken = default)
        {
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var user = await dbContext.UsersEntities
            .FirstOrDefaultAsync(x => x.Id == command.Id);
            if (user == null)
                throw new CustomException("UserEntity Not Found");
            dbContext.Remove(user);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        public async virtual Task Update(UpdateUserCommand command, CancellationToken cancellationToken = default)
        {
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var user = await dbContext.UsersEntities.FirstOrDefaultAsync(x => x.Id == command.UserId, cancellationToken);

            if (user == null)
                throw new CustomException("UserEntity Not Found");

            user.PhoneNumber = command.Entity.PhoneNumber;

            // Generate a new salt
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Hash the password with the new salt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Entity.Password, salt);

            user.Password = passwordHash;

            await dbContext.SaveChangesAsync(cancellationToken);
        }



        #endregion

        #region Helpers

        //[ComputeMethod]
        private List<ProductResultView> Serialize(string json)
            => JsonConvert.DeserializeObject<List<ProductResultView>>(json) ?? [];
        
        public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
        private void Reattach(UserEntity user, UserResultView userView, AppDbContext dbContext)
        {
            UserMapper.FromResult(userView, user);
        }

        private void Sorting(ref IQueryable<UserEntity> unit, TableOptions options) => unit = options.sort_label switch
        {
            "PhoneNumber" => unit.Ordering(options, o => o.PhoneNumber),
            "CreatedAt" => unit.Ordering(options, o => o.CreatedAt),
            _ => unit.OrderBy(o => o.CreatedAt),
        };

        private void Sorting(ref IQueryable<OrderEntity> unit, TableOptions options) => unit = options.sort_label switch
        {
            "City" => unit.Ordering(options, o => o.City),
            "Region" => unit.Ordering(options, o => o.Region),
            "Status" => unit.Ordering(options, o => o.Status),
            "PaymentType" => unit.Ordering(options, o => o.PaymentType),
            "Street" => unit.Ordering(options, o => o.Street),
            "FirstName" => unit.Ordering(options, o => o.FirstName),
            "LastName" => unit.Ordering(options, o => o.LastName),
            _ => unit.OrderBy(o => o.Id),
        };

        #endregion
        #region Token
        public async virtual Task<string> Login(string phoneNumber, string password)
        {
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var user = await dbContext.UsersEntities
                .Where(x => x.PhoneNumber == phoneNumber)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                var command = new CreateUserCommand(Session.Default, new UserResultView { PhoneNumber = phoneNumber, Password = password });
                bool created = await commander.Call<bool>(command);
                if (created == true)
                {
                    var text = GenerateToken(phoneNumber);
                    return text;
                }
                user = await dbContext.UsersEntities
                    .Where(x => x.PhoneNumber == phoneNumber)
                    .FirstOrDefaultAsync();
            }

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new CustomException("Password is incorrect");
            }

            string token = GenerateToken(phoneNumber);
            return token;
        }



        public async Task<string> AdminLogin(string phoneNumber, string password)
        {
            using var dbContext = dbHub.CreateDbContext();
            var user = await dbContext.UsersEntities
                .Where(x => x.PhoneNumber == phoneNumber && x.Role == "Admin")
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new CustomException("Admin user not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new CustomException("Admin user not found");
            }

            return GenerateToken(phoneNumber);
        }

        private string GenerateToken(string phoneNumber)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, phoneNumber),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings:SecretKey").Value!));

            var secretKey = configuration.GetSection("JwtSettings:SecretKey").Value!;

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(10),
                    signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        #endregion

        #region Token Helper
        private UserEntity IsUser(string phoneNumber)
        {
            using var dbContext = dbHub.CreateDbContext();
            var user = dbContext.UsersEntities.FirstOrDefault(x => x.PhoneNumber == phoneNumber && x.Role == "User");
            return user ?? throw new CustomException("Not Permission");
        }

        private bool IsAdminUser(string phoneNumber)
        {
            using var dbContext = dbHub.CreateDbContext();
            var user = dbContext.UsersEntities.FirstOrDefault(x => x.PhoneNumber == phoneNumber && x.Role == "Admin");
            return user != null;
        }
        private string ValidateToken(string token)
        {
            var jwtEncodedString = token.Substring(7);

            var secondToken = new JwtSecurityToken(jwtEncodedString);
            var json = secondToken.Payload.Values.FirstOrDefault();
            if (json == null)
                throw new CustomException("Payload is null");
            else
            {
                return json?.ToString() ?? string.Empty;
            }
        }
        #endregion
    }
}
