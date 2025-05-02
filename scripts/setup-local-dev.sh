#!/bin/bash

# ----------------------------------------------------------------
# Waypoint Local Development Setup Script
# 
# This script helps set up your local development environment
# and provides guidance on using GitHub Actions for common tasks.
# ----------------------------------------------------------------

# Define color codes for better readability
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Determine script and project root directories
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" &> /dev/null && pwd)"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

echo -e "${BLUE}=====================================================${NC}"
echo -e "${GREEN}Welcome to Waypoint Local Development Setup!${NC}"
echo -e "${BLUE}=====================================================${NC}"

# Check for Docker availability
if command -v docker &> /dev/null; then
    echo -e "${GREEN}✅ Docker is installed${NC}"
    DOCKER_AVAILABLE=true
else
    echo -e "${YELLOW}⚠️  Docker not found. Some development features may be limited.${NC}"
    echo -e "   Consider installing Docker for containerized development."
    DOCKER_AVAILABLE=false
fi

# Check for environment file
if [ -f "$ROOT_DIR/.env" ]; then
    echo -e "${GREEN}✅ Environment file (.env) exists${NC}"
else
    echo -e "${YELLOW}⚠️  Environment file (.env) not found. Creating from template...${NC}"
    
    if [ -f "$ROOT_DIR/.env.example" ]; then
        cp "$ROOT_DIR/.env.example" "$ROOT_DIR/.env"
        echo -e "${GREEN}✅ Created .env file from template${NC}"
        echo -e "${YELLOW}⚠️  Please update the .env file with your credentials:${NC}"
        echo -e "   ${BLUE}$ROOT_DIR/.env${NC}"
    else
        echo -e "${RED}❌ Error: .env.example template not found!${NC}"
        echo -e "   Please create a .env file manually with required configuration."
        exit 1
    fi
fi

# Check for database configuration
echo -e "\n${BLUE}Checking database configuration...${NC}"
DB_CONNECTION=$(grep DB_CONNECTION_STRING "$ROOT_DIR/.env" | cut -d '=' -f2)

if [ -z "$DB_CONNECTION" ]; then
    echo -e "${YELLOW}⚠️  Database connection string not configured in .env file${NC}"
    echo -e "   Please set up your database and update the DB_CONNECTION_STRING in your .env file."
    
    if [ "$DOCKER_AVAILABLE" = true ]; then
        echo -e "\n${GREEN}Would you like to start a PostgreSQL database using Docker? (y/n)${NC}"
        read -r start_db
        
        if [[ $start_db =~ ^[Yy]$ ]]; then
            echo -e "${BLUE}Starting PostgreSQL database in Docker...${NC}"
            docker run --name waypoint-postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_USER=postgres -e POSTGRES_DB=waypoint -p 5432:5432 -d postgres:14
            
            if [ $? -eq 0 ]; then
                echo -e "${GREEN}✅ PostgreSQL database started successfully${NC}"
                echo -e "${YELLOW}⚠️  Update your .env file with the following connection string:${NC}"
                echo -e "   DB_CONNECTION_STRING=postgresql://postgres:postgres@localhost:5432/waypoint"
            else
                echo -e "${RED}❌ Failed to start PostgreSQL database${NC}"
                echo -e "   Please check Docker configuration or set up your database manually."
            fi
        fi
    fi
else
    echo -e "${GREEN}✅ Database connection string is configured${NC}"
fi

# Display helpful information about GitHub Actions
echo -e "\n${BLUE}=====================================================${NC}"
echo -e "${GREEN}GitHub Actions Available for Waypoint Development${NC}"
echo -e "${BLUE}=====================================================${NC}"
echo -e "The following GitHub Actions workflows are available:"
echo -e ""
echo -e "${GREEN}1. Apply Database Migrations${NC}"
echo -e "   Use this workflow to set up or update your database schema"
echo -e ""
echo -e "${GREEN}2. Revert Database Migrations${NC}"
echo -e "   Use this workflow to revert database changes if needed"
echo -e ""
echo -e "${GREEN}3. Run Database Query${NC}"
echo -e "   Use this workflow to execute SQL queries against your database"
echo -e ""
echo -e "${GREEN}4. Create Database Backup${NC}"
echo -e "   Use this workflow to back up your database"
echo -e ""
echo -e "${GREEN}5. Setup Development Environment${NC}"
echo -e "   Use this workflow to set up a new development environment"
echo -e ""
echo -e "${BLUE}For more information, see:${NC}"
echo -e "  - ${YELLOW}.github/workflows/README.md${NC} (Workflow documentation)"
echo -e "  - ${YELLOW}.github/DEVELOPMENT.md${NC} (Development guide)"
echo -e "${BLUE}=====================================================${NC}"

echo -e "\n${GREEN}Local development environment setup complete!${NC}"
echo -e "You can now start developing with Waypoint."
echo -e "\n${YELLOW}Next steps:${NC}"
echo -e "1. Update your .env file with your credentials"
echo -e "2. Apply database migrations using the GitHub Actions workflow"
echo -e "3. Start building your application"
echo -e "\n${BLUE}Happy coding!${NC}\n" 