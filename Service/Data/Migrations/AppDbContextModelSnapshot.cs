﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Service.Data;

#nullable disable

namespace Service.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.2.23480.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Shared.Features.AddressEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("district");

                    b.Property<string>("DomophoneCode")
                        .HasColumnType("text")
                        .HasColumnName("domophone_code");

                    b.Property<string>("HomeNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("home_number");

                    b.Property<int>("HomeOrOffice")
                        .HasColumnType("integer")
                        .HasColumnName("home_or_office");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("region");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("street");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("addresses");
                });

            modelBuilder.Entity("Shared.Features.BannerEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("link");

                    b.Property<string>("Locale")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("locale");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("photo");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("banners");
                });

            modelBuilder.Entity("Shared.Features.BrandEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("ImageOne")
                        .HasColumnType("text")
                        .HasColumnName("photo");

                    b.Property<string>("IsPopular")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("is_popular");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("brands");
                });

            modelBuilder.Entity("Shared.Features.CartEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<string>("Product")
                        .HasColumnType("jsonb")
                        .HasColumnName("products");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("carts");
                });

            modelBuilder.Entity("Shared.Features.CourierEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("middle_name");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint")
                        .HasColumnName("order_id");

                    b.Property<string>("PassportLetter")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passport_letter");

                    b.Property<string>("PassportNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passport_number");

                    b.Property<string>("PassportPINFL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passport_PINFL");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("couriers");
                });

            modelBuilder.Entity("Shared.Features.FavouriteEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<List<long>>("Products")
                        .IsRequired()
                        .HasColumnType("bigint[]")
                        .HasColumnName("product_ids");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("favourites");
                });

            modelBuilder.Entity("Shared.Features.FileEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Extension")
                        .HasColumnType("text")
                        .HasColumnName("extension");

                    b.Property<Guid?>("FileId")
                        .HasColumnType("uuid")
                        .HasColumnName("file_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Path")
                        .HasColumnType("text")
                        .HasColumnName("path");

                    b.Property<long>("Size")
                        .HasColumnType("bigint")
                        .HasColumnName("size");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("files");
                });

            modelBuilder.Entity("Shared.Features.OrderEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<string>("CommentForCourier")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("comment_for_courier");

                    b.Property<long?>("CourierEntityId")
                        .HasColumnType("bigint");

                    b.Property<string>("DeliveryTime")
                        .HasColumnType("text")
                        .HasColumnName("delivery_time");

                    b.Property<string>("ExtraPhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extra_phone_number");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("HomeNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("home_number");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("PaymentType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("payment_type");

                    b.Property<string>("Products")
                        .HasColumnType("jsonb")
                        .HasColumnName("products");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("region");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("street");

                    b.Property<long?>("UserEntityId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("CourierEntityId");

                    b.HasIndex("UserEntityId");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("Shared.Features.ProductCategoryEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("NameRu")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name_ru");

                    b.Property<string>("NameUz")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name_uz");

                    b.HasKey("Id");

                    b.ToTable("product_category");
                });

            modelBuilder.Entity("Shared.Features.ProductEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("brand_name");

                    b.Property<long?>("CartId")
                        .HasColumnType("bigint");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("category");

                    b.Property<int?>("Count")
                        .HasColumnType("integer")
                        .HasColumnName("count");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("DescriptionRu")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description_ru");

                    b.Property<string>("DescriptionUz")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description_uz");

                    b.Property<decimal>("DiscountPercent")
                        .HasColumnType("numeric")
                        .HasColumnName("discount_percent");

                    b.Property<decimal>("DiscountPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("discount_price");

                    b.Property<long?>("FavouriteId")
                        .HasColumnType("bigint");

                    b.Property<int?>("InfoCount")
                        .HasColumnType("integer")
                        .HasColumnName("info_count");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsDeliveryFree")
                        .HasColumnType("boolean")
                        .HasColumnName("is_delivery_free");

                    b.Property<int>("MaxCount")
                        .HasColumnType("integer")
                        .HasColumnName("max_count");

                    b.Property<string>("NameRu")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name_ru");

                    b.Property<string>("NameUz")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name_uz");

                    b.Property<string>("PhotoFive")
                        .HasColumnType("text")
                        .HasColumnName("photo_5");

                    b.Property<string>("PhotoFour")
                        .HasColumnType("text")
                        .HasColumnName("photo_4");

                    b.Property<string>("PhotoOne")
                        .HasColumnType("text")
                        .HasColumnName("photo_1");

                    b.Property<string>("PhotoSix")
                        .HasColumnType("text")
                        .HasColumnName("photo_6");

                    b.Property<string>("PhotoThree")
                        .HasColumnType("text")
                        .HasColumnName("photo_3");

                    b.Property<string>("PhotoTwo")
                        .HasColumnType("text")
                        .HasColumnName("photo_2");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<string>("PriceType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("price_type");

                    b.Property<string>("SubCategory")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sub_category");

                    b.Property<string>("Tag")
                        .HasColumnType("text")
                        .HasColumnName("tag");

                    b.Property<string>("Unit")
                        .HasColumnType("text")
                        .HasColumnName("unit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<decimal>("Weight")
                        .HasColumnType("numeric")
                        .HasColumnName("weight");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("FavouriteId");

                    b.ToTable("products");
                });

            modelBuilder.Entity("Shared.Features.ProductSubCategoryEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint")
                        .HasColumnName("category_id");

                    b.Property<string>("Href")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("href");

                    b.Property<string>("NameRu")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name_ru");

                    b.Property<string>("NameUz")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name_uz");

                    b.HasKey("Id");

                    b.ToTable("product_sub_category");
                });

            modelBuilder.Entity("Shared.Features.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<string>("Role")
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("project_users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2024, 3, 7, 10, 52, 52, 754, DateTimeKind.Utc).AddTicks(3691),
                            Password = "$2a$11$h0REc2wxjzhs0NQ9t/HOtutNjgz5P6WhfjOF.7PSUngzVAxQrBN3.",
                            PhoneNumber = "Admin",
                            Role = "Admin"
                        });
                });

            modelBuilder.Entity("Stl.Fusion.Authentication.Services.DbSessionInfo<string>", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("AuthenticatedIdentity")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsSignOutForced")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastSeenAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OptionsJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<long>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt", "IsSignOutForced");

                    b.HasIndex("IPAddress", "IsSignOutForced");

                    b.HasIndex("LastSeenAt", "IsSignOutForced");

                    b.HasIndex("UserId", "IsSignOutForced");

                    b.ToTable("_Sessions");
                });

            modelBuilder.Entity("Stl.Fusion.Authentication.Services.DbUser<string>", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ClaimsJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Stl.Fusion.Authentication.Services.DbUserIdentity<string>", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("DbUserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("UserId");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DbUserId");

                    b.HasIndex("Id");

                    b.ToTable("UserIdentities");
                });

            modelBuilder.Entity("Stl.Fusion.EntityFramework.Operations.DbOperation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AgentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CommandJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CommitTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ItemsJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "CommitTime" }, "IX_CommitTime");

                    b.HasIndex(new[] { "StartTime" }, "IX_StartTime");

                    b.ToTable("_Operations");
                });

            modelBuilder.Entity("Stl.Fusion.Extensions.Services.DbKeyValue", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Key");

                    b.HasIndex("ExpiresAt");

                    b.ToTable("_KeyValues");
                });

            modelBuilder.Entity("Shared.Features.AddressEntity", b =>
                {
                    b.HasOne("Shared.Features.UserEntity", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Shared.Features.CartEntity", b =>
                {
                    b.HasOne("Shared.Features.OrderEntity", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId");

                    b.HasOne("Shared.Features.UserEntity", "User")
                        .WithOne("Cart")
                        .HasForeignKey("Shared.Features.CartEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Shared.Features.FavouriteEntity", b =>
                {
                    b.HasOne("Shared.Features.UserEntity", "User")
                        .WithOne("Favourite")
                        .HasForeignKey("Shared.Features.FavouriteEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Shared.Features.OrderEntity", b =>
                {
                    b.HasOne("Shared.Features.CourierEntity", null)
                        .WithMany("Orders")
                        .HasForeignKey("CourierEntityId");

                    b.HasOne("Shared.Features.UserEntity", null)
                        .WithMany("Orders")
                        .HasForeignKey("UserEntityId");
                });

            modelBuilder.Entity("Shared.Features.ProductEntity", b =>
                {
                    b.HasOne("Shared.Features.CartEntity", "Cart")
                        .WithMany("Products")
                        .HasForeignKey("CartId");

                    b.HasOne("Shared.Features.FavouriteEntity", "Favourite")
                        .WithMany("ProductEntity")
                        .HasForeignKey("FavouriteId");

                    b.Navigation("Cart");

                    b.Navigation("Favourite");
                });

            modelBuilder.Entity("Stl.Fusion.Authentication.Services.DbUserIdentity<string>", b =>
                {
                    b.HasOne("Stl.Fusion.Authentication.Services.DbUser<string>", null)
                        .WithMany("Identities")
                        .HasForeignKey("DbUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Features.CartEntity", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Shared.Features.CourierEntity", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Shared.Features.FavouriteEntity", b =>
                {
                    b.Navigation("ProductEntity");
                });

            modelBuilder.Entity("Shared.Features.UserEntity", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Cart");

                    b.Navigation("Favourite");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Stl.Fusion.Authentication.Services.DbUser<string>", b =>
                {
                    b.Navigation("Identities");
                });
#pragma warning restore 612, 618
        }
    }
}
