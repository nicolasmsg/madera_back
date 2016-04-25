# README #

###Within SQL Server Management Studio
* Drop any existing ConceptionDevis database on this server
* Create a user in SQLServer express (your instance must be localhost\SQLEXpress, otherwise customize the connectionString in web.config)
* Dans SQL Server Management Studio 
    * localhost\SQLExpress 
        * Securiy 
            * Connections 
            * right click > create new connection
            * name iisback > password iisback1234 (x2, confirm)
            * server roles > tick dbcreator

###Within Visual Studio (2015 recommended)
* Open the ConceptionDevisWS solution in Visual Studio (you must have .net framework 4.5.2 installed)
* right click on your project > manage NuGet packages ...
* click on the restore button
* right click on the project > clean
* right click on the project > rebuild
* run the project
* open your browser on http://localhost:59560/
you should get an error 401.13 and your setup

then, please read the APIList.xlsx document at the root of the git repository to see available uri and how to use them.