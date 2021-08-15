#!/bin/sh
# Update all SubModules to latest and commit

cd swagger-ui
git checkout master
git pull
cd ..

cd swagger-ui-themes
git checkout develop
git pull
cd ..

git commit -am "Update Swagger-UI"
