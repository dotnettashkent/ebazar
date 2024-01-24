using System.IdentityModel.Tokens.Jwt;
using System.Text;

// A jwt encoded token string in this case extracted from the 'Authorization' HTTP header
var tokenString = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE3MDQ4NzgyOTksImV4cCI6MTczNjQxNDI5OSwiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2tldEBleGFtcGxlLmNvbSIsIkdpdmVuTmFtZSI6IkpvaG5ueSIsIlN1cm5hbWUiOiJSb2NrZXQiLCJFbWFpbCI6Impyb2NrZXRAZXhhbXBsZS5jb20iLCJSb2xlIjpbIk1hbmFnZXIiLCJQcm9qZWN0IEFkbWluaXN0cmF0b3IiXX0.UEu_ezuiuCIyXaD5-Vh_xt1qh3TYk3rc2UaiKIbw4m0";

// Trim 'Bearer ' from the start since its just a prefix for the token
var jwtEncodedString = tokenString.Substring(7);

// Instantiate a new Jwt Security Token from the Jwt Encoded String
var token = new JwtSecurityToken(jwtEncodedString);

// Retrieve info from the Json Web Token 
Console.WriteLine("email => " + token.Claims.FirstOrDefault(c => c.Type == "iss").Value);
