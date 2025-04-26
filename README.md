Steps to Set Up
---------------

1. Clone the Repository and Extract the Database Files.
	- Clone the git repository
	- Open the project in Visual Studio 2022
	- Find and extract the Database.zip file in the repository
	
2. Attach the Database in Visual Studio.
	- Open Server Explorer (View > Server Explorer) in Visual Studio
	- Right-click on Data Connections and select Add Connection
	- In the window that appears:
		- Set the Data source to Microsoft SQL Server Database Files
		- Browse and add the .mdf file extracted from the repository
		- Use Windows authentication
		- Click Ok to attach

3. Update the connection string.
	- Open appsettings.json in the Visual Studio project
	- Update the PlanitGreenConnection to match the attached database
		- To get the correct connection string:
			--> In Server Explorer, right-click the newly attached database under Data Connections
			--> Select Properties.
			--> Copy the Connection String shown in the Properties window
	
4. Run the project.
	- Build and run the project from Visual Studio

Note:
If NuGet packages do not automatically restore, go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution and click Restore.

Prerequisites:
Visual Studio 2022 installed (With ASP.NET and web development workload).
SQL Server LocalDB installed (Usually comes by default with Visual Studio).
