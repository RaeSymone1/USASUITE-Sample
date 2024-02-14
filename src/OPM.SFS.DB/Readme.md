#Helpful dotnet Commands for working with this project#
- This project is configured to run for SQL 2019!
- Will need to upgrade your local to 2019 - https://medium.com/cloudnimble/upgrade-visual-studio-2019s-localdb-to-sql-2019-da9da71c8ed6
- https://knowledge-base.havit.eu/2020/12/17/sql-localdb-upgrade-to-2019-15-0-2000/

###Deploy to local###
dotnet publish /p:TargetServerName=(localdb)\MSSQLLocalDB /p:TargetDatabaseName=ScholarshipForService


#Pre-deploy and post-deploy script naming convention#
- Release: Current sprint with no period (ex Sprint 8.5)
- Sequence: If scripts need to run in a sequence use this format to determine script order. Ran in asc order (001, 002, 003)
- Date: Format yyyymmdd
- Scriptname: Short descriptive name of the scripts purpose

Format
[Release].[Date].[Sequence] -[Scriptname]

Example
84.20211009.001 - Adding new institutions.sql