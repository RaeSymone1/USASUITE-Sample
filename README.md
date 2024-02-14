# Introduction 

Scholarship For Service (SFS) is a program designed to recruit and train the next generation of IT professionals to meet the needs of the cybersecurity mission for Federal, 
State, local, and tribal governments. The program provides scholarships up to 3 years of support for cybersecurity undergraduate or graduate (MS or PhD) education. 
n return for their scholarships, recipients must agree to work after graduation for the U.S. Government, in a position related to cybersecurity, for a period equal to the length of the scholarship.

# Getting Started
The SFS application is an ASP.NET Core Razor Pages project that utilizes EF Core for database access. SFS also utilizes a "vertical slice" architecture utilzing a library
called Mediatr to facilate loosley coupled queries and commands to support the various actions on the view's page model. 

The following is needed to get started.
1.	Repo access and cloned
2.	Local database set up, backup restored.
3.	Obtain secrets from another developer
4.	Build solution. Run database migrations


# SFS Sites

| Environment  | Database | Url |
| :---------------- | :------: | ----: |
| Local  | (localdb)\MSSQLLocalDB   | https://localhost:5001 |
| DEV    | sqlmi-sfs-dev-e2-001.b99e104f412f.database.windows.net   | https://sfsdev.opm.gov/ |
| TEST   | sqlmi-sfs-test-e2-001.17ddc25c7056.database.windows.net   | https://sfstest.opm.gov/ |
| UAT    | sqlmi-sfs-uat-e2-001.443d4a23c338.database.windows.net   | https://sfsuat.opm.gov/ |
| PRD    | sqlmi-sfs-uat-e2-001.443d4a23c338.database.windows.net   | https://sfs.opm.gov/ |



# Helpful links

- Learn Razor Pages - https://www.learnrazorpages.com/
- Learn EF Core - https://www.learnentityframeworkcore.com/
- MediatR - https://github.com/jbogard/MediatR
- Vertical Slice Architecture - https://www.jimmybogard.com/vertical-slice-architecture/
- Vertical Slice Architecture (updated) - https://www.jimmybogard.com/vertical-slice-example-updated-to-net-5/
- Refactoring Guide - https://refactoring.guru/refactoring
