#!/bin/sh

uname
sudo cp -rf src/Pundit.Web.Linux/* /var/www/pundit-dev.com/ 
sudo service apache2 restart

