# FundApps - Task - Jamie Hayes
The application was written in C# in Visual Studio Code (v1.66.0) with the following extensions; .NET Core Test Explorer (v0.7.7), C# (v1.24.3), C# Extensions (v1.3.1)
Each project is using NET Core 5.0	

Api interaction and Unit tests were used to verify the functionality of the application

# Step 1
Added service and handling for parcel pricing with just dimenions, with unit tests

# Step 2
Added property to display speedy shipping price, with unit tests

# Step 3
Restructured models to handle weighting plus additional costs and added to service, with unit tests

# Step 5
Restructured models to handle discounts as well as new services. Added discount for small parcels
Added discount for medium/mixed parcels
Rearranged discount order and applied new property to track which parcels are discounted to stop duplicates

# Step 4
Added heavy parcel type and handling for choosing between marking as heavy or standard parcel.

# Future work
I would like to look to integrate a database to store possible parcel types, which would allow for more refactoring in current services.
There is currently quite a lot of duplication for the individual discounts, so I would like to maybe make use of database options and maybe a parent class
Would be great to have a basic web ui to display to the users potential different options and they can see what happens when changing dimensions/weights