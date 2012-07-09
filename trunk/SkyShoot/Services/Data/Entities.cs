//------------------------------------------------------------------------------
// <auto-generated>
//	 This code was generated from a template.
//
//	 Changes to this file may cause incorrect behavior and will be lost if
//	 the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// Architectural overview and usage guide: 
// http://blogofrab.blogspot.com/2010/08/maintenance-free-mocking-for-unit.html
//------------------------------------------------------------------------------
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Common;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace Services.Data
{
    /// <summary>
    /// The functional concrete object context. This is just like the normal
    /// context that would be generated using the POCO artefact generator, 
    /// apart from the fact that this one implements an interface containing 
    /// the entity set properties and exposes <code>IObjectSet</code>
    /// instances for entity set properties.
    /// </summary>
    [GeneratedCode("Entity","0.9"), DebuggerNonUserCode]
    public partial class Entities : ObjectContext, IEntities 
    {
    	public const string ConnectionString = "name=Entities";
    	public const string ContainerName = "Entities";
    
    	#region Constructors
    
    	public Entities():
    		base(ConnectionString, ContainerName)
    	{
    		this.ContextOptions.LazyLoadingEnabled = true;
    	}
    
    	public Entities(string connectionString):
    		base(connectionString, ContainerName)
    	{
    		this.ContextOptions.LazyLoadingEnabled = true;
    	}
    
    	public Entities(EntityConnection connection):
    		base(connection, ContainerName)
    	{
    		this.ContextOptions.LazyLoadingEnabled = true;
    	}
    
    	#endregion
    
    	#region IEntities members
    
    	public void UseTracking(bool enable)
    	{
    		this.ContextOptions.ProxyCreationEnabled = enable;
    	}
    
    	#endregion
    
        #region ObjectSet Properties
    
    	public IObjectSet<Administrator> Administrators
    	{
    		get { return _administrators ?? (_administrators = CreateObjectSet<Administrator>("Administrators")); }
    	}
    	private ObjectSet<Administrator> _administrators;
    
    	public IObjectSet<User> Users
    	{
    		get { return _users ?? (_users = CreateObjectSet<User>("Users")); }
    	}
    	private ObjectSet<User> _users;

        #endregion
    }
}