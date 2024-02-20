-- Create Users table
CREATE TABLE "Users" (
    "Id" text NOT NULL,
    "Version" bigint NOT NULL,
    "Name" text NOT NULL,
    "ClaimsJson" text NOT NULL,
    PRIMARY KEY ("Id")
);

-- Create _KeyValues table
CREATE TABLE "_KeyValues" (
    "Key" text NOT NULL,
    "Value" text NOT NULL,
    "ExpiresAt" timestamptz NULL,
    PRIMARY KEY ("Key")
);

-- Create _Operations table
CREATE TABLE "_Operations" (
    "Id" text NOT NULL,
    "AgentId" text NOT NULL,
    "StartTime" timestamptz NOT NULL,
    "CommitTime" timestamptz NOT NULL,
    "CommandJson" text NOT NULL,
    "ItemsJson" text NOT NULL,
    PRIMARY KEY ("Id")
);

-- Create _Sessions table
CREATE TABLE "_Sessions" (
    "Id" varchar(256) NOT NULL,
    "Version" bigint NOT NULL,
    "CreatedAt" timestamptz NOT NULL,
    "LastSeenAt" timestamptz NOT NULL,
    "IPAddress" text NOT NULL,
    "UserAgent" text NOT NULL,
    "AuthenticatedIdentity" text NOT NULL,
    "UserId" text NULL,
    "IsSignOutForced" boolean NOT NULL,
    "OptionsJson" text NOT NULL,
    PRIMARY KEY ("Id")
);

-- Create banners table
CREATE TABLE "banners" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "locale" text NOT NULL,
    "photo" text NOT NULL,
    "title" text NOT NULL,
    "link" text NOT NULL,
    "description" text NOT NULL,
    "created_at" timestamptz NOT NULL,
    "updated_at" timestamptz NULL,
    PRIMARY KEY ("id")
);

-- Create brands table
CREATE TABLE "brands" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "name" text NOT NULL,
    "is_popular" text NOT NULL,
    "Link" text NOT NULL,
    "photo" text NULL,
    PRIMARY KEY ("id")
);

-- Create couriers table
CREATE TABLE "couriers" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "order_id" bigint NOT NULL,
    "last_name" text NOT NULL,
    "first_name" text NOT NULL,
    "middle_name" text NOT NULL,
    "phone_number" text NOT NULL,
    "password" text NOT NULL,
    "passport_number" text NOT NULL,
    "passport_letter" text NOT NULL,
    "passport_PINFL" text NOT NULL,
    "created_at" timestamptz NOT NULL,
    "updated_at" timestamptz NULL,
    PRIMARY KEY ("id")
);

-- Create files table
CREATE TABLE "files" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "name" text NOT NULL,
    "file_id" uuid NULL,
    "extension" text NULL,
    "path" text NULL,
    "size" bigint NOT NULL,
    "type" integer NOT NULL,
    "created_at" timestamptz NOT NULL,
    "updated_at" timestamptz NULL,
    PRIMARY KEY ("id")
);

-- Create product_category table
CREATE TABLE "product_category" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "name" text NOT NULL,
    PRIMARY KEY ("id")
);

-- Create product_sub_category table
CREATE TABLE "product_sub_category" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "name" text NOT NULL,
    PRIMARY KEY ("id")
);

-- Create project_users table
CREATE TABLE "project_users" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "phone_number" text NOT NULL,
    "password" text NOT NULL,
    "role" text NULL,
    "created_at" timestamptz NULL,
    "updated_at" timestamptz NULL,
    PRIMARY KEY ("id")
);

-- Create UserIdentities table
CREATE TABLE "UserIdentities" (
    "Id" text NOT NULL,
    "UserId" text NOT NULL,
    "Secret" text NOT NULL,
    PRIMARY KEY ("Id"),
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

-- Create addresses table
CREATE TABLE "addresses" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "user_id" bigint NOT NULL,
    "region" text NOT NULL,
    "district" text NOT NULL,
    "street" text NOT NULL,
    "home_number" text NOT NULL,
    "home_or_office" integer NOT NULL,
    "domophone_code" text NULL,
    PRIMARY KEY ("id"),
    FOREIGN KEY ("user_id") REFERENCES "project_users" ("id") ON DELETE CASCADE
);

-- Create favourites table
CREATE TABLE "favourites" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "user_id" bigint NOT NULL,
    "product_ids" bigint[] NOT NULL,
    PRIMARY KEY ("id"),
    FOREIGN KEY ("user_id") REFERENCES "project_users" ("id") ON DELETE CASCADE
);

-- Create orders table
CREATE TABLE "orders" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "user_id" bigint NOT NULL,
    "city" text NOT NULL,
    "region" text NOT NULL,
    "street" text NOT NULL,
    "home_number" text NOT NULL,
    "comment_for_courier" text NOT NULL,
    "delivery_time" text NULL,
    "payment_type" text NOT NULL,
    "first_name" text NOT NULL,
    "last_name" text NOT NULL,
    "extra_phone_number" text NOT NULL,
    "status" text NULL,
    "products" jsonb NULL,
    "CourierEntityId" bigint NULL,
    "UserEntityId" bigint NULL,
    PRIMARY KEY ("id"),
    FOREIGN KEY ("CourierEntityId") REFERENCES "couriers" ("id"),
    FOREIGN KEY ("UserEntityId") REFERENCES "project_users" ("id")
);

-- Create carts table
CREATE TABLE "carts" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "products" jsonb NULL,
    "user_id" bigint NOT NULL,
    "OrderId" bigint NULL,
    PRIMARY KEY ("id"),
    FOREIGN KEY ("OrderId") REFERENCES "orders" ("id"),
    FOREIGN KEY ("user_id") REFERENCES "project_users" ("id") ON DELETE CASCADE
);

-- Create products table
CREATE TABLE "products" (
    "id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    "name_uz" text NOT NULL,
    "name_ru" text NOT NULL,
    "description_uz" text NOT NULL,
    "description_ru" text NOT NULL,
    "brand_name" text NOT NULL,
    "count" integer NULL,
    "max_count" integer NOT NULL,
    "info_count" integer NULL,
    "price" numeric NOT NULL,
    "discount_price" numeric NOT NULL,
    "discount_percent" numeric NOT NULL,
    "price_type" text NOT NULL,
    "is_delivery_free" boolean NOT NULL,
    "photo_1" text NULL,
    "photo_2" text NULL,
    "photo_3" text NULL,
    "photo_4" text NULL,
    "photo_5" text NULL,
    "photo_6" text NULL,
    "tag" text NULL,
    "weight" numeric NOT NULL,
    "unit" text NULL,
    "is_active" boolean NOT NULL,
    "category" text NOT NULL,
    "sub_category" text NOT NULL,
    "created_at" timestamptz NOT NULL,
    "updated_at" timestamptz NULL,
    "CartId" bigint NULL,
    "FavouriteId" bigint NULL,
    PRIMARY KEY ("id"),
    FOREIGN KEY ("CartId") REFERENCES "carts" ("id"),
    FOREIGN KEY ("FavouriteId") REFERENCES "favourites" ("id")
);

INSERT INTO project_users (phone_number, password, role, created_at, updated_at)
VALUES ('Admin', '$2a$12$bbbRogEywJVmBdwScd3kYewXM4A23byFBqP1O13rzb5BDjs7C56cm', 'Admin', current_timestamp, NULL);
