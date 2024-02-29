#!/bin/bash

# Function to update the application code
update_code() {
    echo "Changing directory to the project..."
    if cd ~/projects/ebazar; then
        echo "Pulling latest changes from Git..."
        if git pull; then
            echo "Latest changes pulled successfully."
        else
            echo "Failed to pull changes from Git. Exiting..."
            exit 1
        fi
    else
        echo "Failed to change directory. Exiting..."
        exit 1
    fi
}

# Function to publish the application
publish_app() {
    echo "Publishing the new application..."
    if dotnet publish; then
        echo "Application published successfully."
    else
        echo "Failed to publish the application. Exiting..."
        exit 1
    fi
}

# Function to stop the service
stop_service() {
    echo "Stopping ebazar.service..."
    if sudo systemctl stop entity1.service; then
        echo "Ebazar Service stopped successfully."
    else
        echo "Failed to stop ebazar.service. Exiting..."
        exit 1
    fi
}

# Function to clean old application files
clean_old_files() {
    echo "Removing old application files..."
    if sudo rm -rf /var/www/ebazar/*; then
        echo "Old files removed successfully."
    else
        echo "Failed to remove old files. Exiting..."
        exit 1
    fi
}

# Function to deploy the application
deploy_app() {
    echo "Copying new files to deployment directory..."
    if sudo cp -r Server/bin/Release/net8.0/publish/* /var/www/ebazar/; then
        echo "New files copied successfully."
    else
        echo "Failed to copy new files. Exiting..."
        exit 1
    fi
}

# Function to start the service
start_service() {
    echo "Starting ebazar.service..."
    if sudo systemctl start entity1.service; then
        echo "Ebazar Service started successfully."
    else
        echo "Failed to start entity1.service. Exiting..."
        exit 1
    fi
}

# Main script execution starts here
echo "Deployment process started."

update_code
publish_app
stop_service
clean_old_files
deploy_app
start_service

echo "Deployment completed successfully."
