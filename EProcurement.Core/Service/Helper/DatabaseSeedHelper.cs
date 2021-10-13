using EProcurement.Common;
using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace EProcurement.Core
{
    public class DatabaseSeedHelper
    {
        public void SeedData()
        {
            
        }
        private readonly ApplicationDbContext _db;
        public DatabaseSeedHelper(ApplicationDbContext db)
        {
            _db = db;
        }
    }

    public class PriveliedgeHelper
    {
        private readonly ApplicationDbContext _db;
        public PriveliedgeHelper(ApplicationDbContext db)
        {
            _db = db;
        }

        public void SeedDbClientPriviledge(List<ClientPrivilege> clientPrivileges)
        {
            foreach (var privilaeg in clientPrivileges)
            {
                var prevPrivilaegs = _db.ClientPrivilege.Where(c => c.Action == privilaeg.Action).FirstOrDefault();

                if (prevPrivilaegs == null)
                {
                    var newPrivelesge = new ClientPrivilege
                    {
                        Action = privilaeg.Action,
                        Module = privilaeg.Module,
                        RecordStatus = RecordStatus.Active,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.MaxValue,
                    };
                    _db.ClientPrivilege.Add(newPrivelesge);
                }
            }
            _db.SaveChanges();
        }

        public void SeedDbPriviledge(List<Privilege> privileges)
        {
            foreach (var privilaeg in privileges)
            {
                var prevPrivilaegs = _db.Privilege.Where(c => c.Action == privilaeg.Action).FirstOrDefault();

                if (prevPrivilaegs == null)
                {
                    var newPrivelesge = new Privilege
                    {
                        Action = privilaeg.Action,
                        Module = privilaeg.Module,
                        IsMorePermission = privilaeg.IsMorePermission,                      
                        RecordStatus = RecordStatus.Active,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.MaxValue,
                    };
                    _db.Privilege.Add(newPrivelesge);
                }
            }
            _db.SaveChanges();
        }
        public void SeedDbClientPriviledgeRoleCombination(ClientPrivelidgeRoleCreationRequest clientPrivelidgeRoleCreation)
        {
            //get list of privelidge
            var listOfClientPriviledge = _db.ClientPrivilege.ToList();

            foreach (var privilaeg in listOfClientPriviledge)
            {
                var prevRolePrivilaegs = _db.ClientRolePrivilege.Where(c => c.RoleId == clientPrivelidgeRoleCreation.RoleId && c.PrivilegeId == privilaeg.Id).FirstOrDefault();

                if (prevRolePrivilaegs == null)
                {
                    var rolePrivelesge = new ClientRolePrivilege
                    {
                        PrivilegeId = privilaeg.Id,
                        RoleId = clientPrivelidgeRoleCreation.RoleId,
                        RecordStatus = RecordStatus.Active,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.MaxValue,
                    };
                    _db.ClientRolePrivilege.Add(rolePrivelesge);
                }
            }
            _db.SaveChanges();
        }
        public void SeedDbPriviledgeRoleCombination(PrivelidgeRoleCreationRequest privelidgeRoleCreation)
        {
            try
            {
                //get list of privelidge
                var listOfPriviledge = _db.Privilege.Where(p => p.ClientUserId == privelidgeRoleCreation.ClientId).ToList();

                foreach (var privilaeg in listOfPriviledge)
                {
                    var prevRolePrivilaegs = _db.RolePrivilege.Where(c => c.RoleId == privelidgeRoleCreation.RoleId && c.PrivilegeId == privilaeg.Id).FirstOrDefault();

                    if (prevRolePrivilaegs == null)
                    {
                        var rolePrivelesge = new RolePrivilege
                        {
                            PrivilegeId = privilaeg.Id,
                            RoleId = privelidgeRoleCreation.RoleId,
                            RecordStatus = RecordStatus.Active,
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.MaxValue,
                        };
                        _db.RolePrivilege.Add(rolePrivelesge);
                    }
                }
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

    public class PrivelidgeCreationRequest
    {
        public long ClientId { get; set; }
        public List<Privilege> priveledges { get; set; }
    }

    public class PrivelidgeRoleCreationRequest
    {
        public long ClientId { get; set; }
        public long RoleId { get; set; }
    }

    public class ClientPrivelidgeRoleCreationRequest
    {
        public long RoleId { get; set; }
    }
}
