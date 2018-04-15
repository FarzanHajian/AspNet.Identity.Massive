AspNet.Identity.Massive
======================

An implementation of ASP.NET Identity 2.0 which uses [Massive ORM](https://github.com/FransBouma/Massive). This work is based on [AspNet.Identity.Dapper](https://github.com/whisperdancer/AspNet.Identity.Dapper).


The main project is AspNet.Identity.Massive. **By default** it uses SQL Server plugin of Massive but you can add a different plugin file to the project under Massive folder and remember if you want to update
Massive itself of change its plugs, you have to change their namespaces to AspNet.Identity.Massive (The default namespace is Massive). This is because your main project might have Massice itself. So to confront
ambiguity, AspNet.Identity. Massive uses a different namespace for its embedded Massive.


SQL scripts are located inside Database project. Web is a ready-to-use sample MVC5 project is which uses Massive instead of EF. UnitTest project needs more work :):):)


**And keep in mind** that AspNet.Identity.Massive uses integer primary keys for Identity tables in contast to Microsoft.AspNet.Identity.EntityFramework (which uses GUID). If you still nedd GUID PKs, you have to
change it yourself.
