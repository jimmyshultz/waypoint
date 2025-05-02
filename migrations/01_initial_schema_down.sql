-- Downgrade script for initial schema

-- Drop all triggers first
DROP TRIGGER IF EXISTS update_stays_updated_at ON "Stays";
DROP TRIGGER IF EXISTS update_hosts_updated_at ON "Hosts";
DROP TRIGGER IF EXISTS update_users_updated_at ON "Users";

-- Drop trigger function
DROP FUNCTION IF EXISTS update_updated_at_column();

-- Drop tables in reverse order (to respect foreign key constraints)
DROP TABLE IF EXISTS "States";
DROP TABLE IF EXISTS "DbVersionInfo";
DROP TABLE IF EXISTS "Stays";
DROP TABLE IF EXISTS "Hosts";
DROP TABLE IF EXISTS "Users";

-- Drop extension
DROP EXTENSION IF EXISTS "uuid-ossp"; 