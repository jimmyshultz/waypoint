-- Initial Schema for Waypoint Application

-- Enable UUID extension
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Users Table
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "UserName" VARCHAR(100) NOT NULL UNIQUE,
    "FirstName" VARCHAR(100) NULL,
    "LastName" VARCHAR(100) NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Hosts Table
CREATE TABLE IF NOT EXISTS "Hosts" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "UserId" UUID NOT NULL,
    "Name" VARCHAR(255) NOT NULL,
    "PhoneNumber" VARCHAR(20) NULL,
    "Email" VARCHAR(255) NULL,
    "AddressLine1" VARCHAR(255) NOT NULL,
    "AddressLine2" VARCHAR(255) NULL,
    "City" VARCHAR(100) NOT NULL,
    "State" VARCHAR(2) NOT NULL,
    "ZipCode" VARCHAR(10) NOT NULL,
    "Latitude" DECIMAL(9,6) NULL,
    "Longitude" DECIMAL(9,6) NULL,
    "Notes" TEXT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Hosts_Users" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

-- Create index on UserId for faster lookups
CREATE INDEX "IX_Hosts_UserId" ON "Hosts" ("UserId");

-- Create spatial index on Latitude and Longitude
CREATE INDEX "IX_Hosts_Location" ON "Hosts" ("Latitude", "Longitude");

-- Create combined index on City and State for location filtering
CREATE INDEX "IX_Hosts_CityState" ON "Hosts" ("City", "State");

-- Stays Table
CREATE TABLE IF NOT EXISTS "Stays" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "HostId" UUID NOT NULL,
    "StartDate" DATE NOT NULL,
    "EndDate" DATE NOT NULL,
    "Rating" INTEGER NULL CHECK ("Rating" >= 1 AND "Rating" <= 5),
    "Notes" TEXT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Stays_Hosts" FOREIGN KEY ("HostId") REFERENCES "Hosts" ("Id") ON DELETE CASCADE
);

-- Create index on HostId for faster lookups
CREATE INDEX "IX_Stays_HostId" ON "Stays" ("HostId");

-- Create index on StartDate for date-based queries
CREATE INDEX "IX_Stays_StartDate" ON "Stays" ("StartDate");

-- Create trigger function to automatically update the UpdatedAt timestamp
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW."UpdatedAt" = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Create triggers for each table
CREATE TRIGGER update_users_updated_at
BEFORE UPDATE ON "Users"
FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_hosts_updated_at
BEFORE UPDATE ON "Hosts"
FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_stays_updated_at
BEFORE UPDATE ON "Stays"
FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column();

-- Create a database version tracking table
CREATE TABLE IF NOT EXISTS "DbVersionInfo" (
    "Id" SERIAL PRIMARY KEY,
    "Version" VARCHAR(50) NOT NULL,
    "AppliedOn" TIMESTAMP NOT NULL DEFAULT NOW(),
    "Description" TEXT NULL
);

-- Insert initial version
INSERT INTO "DbVersionInfo" ("Version", "Description")
VALUES ('1.0.0', 'Initial schema creation');

-- Seed data for US States (reference data)
CREATE TABLE IF NOT EXISTS "States" (
    "Code" VARCHAR(2) PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL
);

INSERT INTO "States" ("Code", "Name") VALUES
('AL', 'Alabama'),
('AK', 'Alaska'),
('AZ', 'Arizona'),
('AR', 'Arkansas'),
('CA', 'California'),
('CO', 'Colorado'),
('CT', 'Connecticut'),
('DE', 'Delaware'),
('FL', 'Florida'),
('GA', 'Georgia'),
('HI', 'Hawaii'),
('ID', 'Idaho'),
('IL', 'Illinois'),
('IN', 'Indiana'),
('IA', 'Iowa'),
('KS', 'Kansas'),
('KY', 'Kentucky'),
('LA', 'Louisiana'),
('ME', 'Maine'),
('MD', 'Maryland'),
('MA', 'Massachusetts'),
('MI', 'Michigan'),
('MN', 'Minnesota'),
('MS', 'Mississippi'),
('MO', 'Missouri'),
('MT', 'Montana'),
('NE', 'Nebraska'),
('NV', 'Nevada'),
('NH', 'New Hampshire'),
('NJ', 'New Jersey'),
('NM', 'New Mexico'),
('NY', 'New York'),
('NC', 'North Carolina'),
('ND', 'North Dakota'),
('OH', 'Ohio'),
('OK', 'Oklahoma'),
('OR', 'Oregon'),
('PA', 'Pennsylvania'),
('RI', 'Rhode Island'),
('SC', 'South Carolina'),
('SD', 'South Dakota'),
('TN', 'Tennessee'),
('TX', 'Texas'),
('UT', 'Utah'),
('VT', 'Vermont'),
('VA', 'Virginia'),
('WA', 'Washington'),
('WV', 'West Virginia'),
('WI', 'Wisconsin'),
('WY', 'Wyoming'),
('DC', 'District of Columbia'),
('PR', 'Puerto Rico'),
('VI', 'Virgin Islands'),
('GU', 'Guam'); 