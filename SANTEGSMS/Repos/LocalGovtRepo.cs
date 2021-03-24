using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using SANTEGSMS.Helpers;
using SANTEGSMS.IRepos;
using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using SANTEGSMS.Reusables;
using SANTEGSMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Repos
{
    public class LocalGovtRepo : ILocalGovtRepo
    {
        private readonly AppDbContext _context;
        public LocalGovtRepo(AppDbContext context)
        {
            _context = context;
        }

        //-------------------------------States---------------------------------------------------------------
        public async Task<GenericRespModel> createStatesAsync(StateReqModel obj)
        {
            try
            {
                foreach (string stateName in obj.StateName)
                {
                    var check = _context.States.Where(x => x.StateName == stateName).FirstOrDefault();

                    if (check == null)
                    {
                        //Save the States
                        var state = new States
                        {
                            StateName = stateName,
                        };

                        await _context.States.AddAsync(state);
                        await _context.SaveChangesAsync();
                    }
                }

                //return the States Created
                var result = from st in _context.States select st;
                            
                return new GenericRespModel { StatusCode = 200, StatusMessage = "States Created Successfully!", Data = result.ToList()};

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

        //-------------------------------Local Govt---------------------------------------------------------------

        public async Task<GenericRespModel> createLocalGovtAsync(LocalGovtReqModel obj)
        {
            try
            {
                var check =  _context.LocalGovt.Where(x => x.LocalGovtName == obj.LocalGovtName && x.StateId == obj.StateId).FirstOrDefault();

                if (check == null)
                {
                    //Save the LocalGovt
                    var localGovt = new LocalGovt
                    {
                        LocalGovtName = obj.LocalGovtName,
                        StateId = obj.StateId,
                    };

                    await _context.LocalGovt.AddAsync(localGovt);
                    await _context.SaveChangesAsync();

                    //return the States Created
                    var result = from st in _context.LocalGovt where st.Id == localGovt.Id
                                 select new
                             {
                                 st.Id,
                                 st.StateId,
                                 st.States.StateName,
                                 st.LocalGovtName
                             };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Local Government Created Successfully!", Data = result.FirstOrDefault() };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Local Government Already Exists!"};

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

        public async Task<GenericRespModel> getLocalGovtByIdAsync(long localGovtId)
        {
            try
            {
                var result = from st in _context.LocalGovt
                             where st.Id == localGovtId
                             select new
                             {
                                 st.Id,
                                 st.StateId,
                                 st.States.StateName,
                                 st.LocalGovtName
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> getAllLocalGovtInStatesAsync(long stateId)
        {
            try
            {
                var result = from st in _context.LocalGovt
                             where st.StateId == stateId
                             select new
                             {
                                 st.Id,
                                 st.StateId,
                                 st.States.StateName,
                                 st.LocalGovtName
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> updateLocalGovtAsync(long localGovtId, LocalGovtReqModel obj)
        {
            try
            {
                //check if the LocalGovt Exists
                var getLocalGovt = _context.LocalGovt.Where(x => x.Id == localGovtId).FirstOrDefault();

                if (getLocalGovt != null)
                {
                    //check if the LocalGovt to be updated already exists
                    var check = _context.LocalGovt.Where(x => x.LocalGovtName == obj.LocalGovtName && x.StateId == obj.StateId).FirstOrDefault();

                    if (check == null)
                    {
                        getLocalGovt.LocalGovtName = obj.LocalGovtName;
                        getLocalGovt.StateId = obj.StateId;

                        await _context.SaveChangesAsync();

                        //return the LocalGovt Updated
                        var result = from st in _context.LocalGovt
                                     where st.Id == getLocalGovt.Id
                                     select new
                                     {
                                         st.Id,
                                         st.StateId,
                                         st.States.StateName,
                                         st.LocalGovtName
                                     };

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Local Government Updated Successfully", Data = result.FirstOrDefault() };
                    }

                    return new GenericRespModel { StatusCode = 409, StatusMessage = "Local Government Already Exists" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No Local Government With the Specified ID" };

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

        public async Task<GenericRespModel> deleteLocalGovtAsync(long localGovtId)
        {
            try
            {
                //check if the LocalGovt Exists
                var localGovt = _context.LocalGovt.Where(x => x.Id == localGovtId).FirstOrDefault();

                if (localGovt != null)
                {
                     _context.LocalGovt.Remove(localGovt);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 409, StatusMessage = "Local Government Deleted Successfully" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No Local Government With the Specified ID" };

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
