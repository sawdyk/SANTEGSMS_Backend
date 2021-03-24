using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using SANTEGSMS.Helpers;
using SANTEGSMS.IRepos;
using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using SANTEGSMS.Reusables;
using SANTEGSMS.Services.Email;
using SANTEGSMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Repos
{
    public class SchoolRolesRepo : ISchoolRolesRepo
    {
        private readonly AppDbContext _context;
        public SchoolRolesRepo(AppDbContext context)
        {
            _context = context;
        }


        //------------------------SchoolRoles------------------------------------------------------------------

        public async Task<GenericRespModel> getAllSchoolRolesAsync()
        {
            try
            {
                var result = from rol in _context.SchoolRoles select rol;

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getSchoolRolesForSchoolUserCreationAsync()
        {
            try
            {
                var result = from rol in _context.SchoolRoles
                             where rol.Id != Convert.ToInt64(EnumUtility.SchoolRoles.ClassTeacher)
                            && rol.Id != Convert.ToInt64(EnumUtility.SchoolRoles.SubjectTeacher)
                            && rol.Id != Convert.ToInt64(EnumUtility.SchoolRoles.SuperAdministrator)
                             select rol;

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getSchoolRolesByRoleIdAsync(long schoolRoleId)
        {
            try
            {
                var result = from rol in _context.SchoolRoles where rol.Id == schoolRoleId select rol;

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> assignRolesToSchoolUsersAsync(AssignRolesReqModel obj)
        {
            try
            {
                GenericRespModel resp = new GenericRespModel();
                var getSchUser = _context.SchoolUsers.Where(s => s.Id == obj.SchoolUserId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();
                if (getSchUser != null)
                {
                    foreach (var rol in obj.RoleIds)
                    {
                        var getRoles = _context.SchoolUserRoles.Where(s => s.UserId == getSchUser.Id && s.RoleId == rol.Id).FirstOrDefault();

                        if (getRoles != null)
                        {
                            //update the roles
                            getRoles.UserId = obj.SchoolUserId;
                            getRoles.RoleId = rol.Id;

                            await _context.SaveChangesAsync();

                            resp.StatusCode = 200;
                            resp.StatusMessage = "Role(s) Assigned to User Updated Successfully";
                        }
                        else
                        {
                            //save new Role
                            var usrRol = new SchoolUserRoles
                            {
                                UserId = obj.SchoolUserId,
                                RoleId = rol.Id,
                                DateCreated = DateTime.Now
                            };

                            await _context.SchoolUserRoles.AddAsync(usrRol);
                            await _context.SaveChangesAsync();

                            resp.StatusCode = 200;
                            resp.StatusMessage = "Role(s) Assigned to User Successfully";
                        }
                    }
                }
                else
                {
                    resp.StatusCode = 200;
                    resp.StatusMessage = "No User With the Specified ID";
                }

                return resp;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> deleteRolesAssignedToSchoolUsersAsync(DeleteRolesAssignedReqModel obj)
        {
            try
            {
                //if user exists
                var getSchUser = _context.SchoolUsers.Where(s => s.Id == obj.SchoolUserId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();
                if (getSchUser != null)
                {
                    //the roles assigned
                    var getUserRole = _context.SchoolUserRoles.Where(s => s.UserId == obj.SchoolUserId && s.RoleId == obj.RoleId).FirstOrDefault();
                    if (getUserRole != null)
                    {
                        _context.SchoolUserRoles.Remove(getUserRole);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Role Assigned to User Deleted Successfully" };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No User/Role With the Specified ID" };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No User With the Specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
    }
}
