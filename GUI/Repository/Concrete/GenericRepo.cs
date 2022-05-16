using M3Y.Entities;
using M3Y.Repository;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace GUI.Repository.Concrete
{
    public class GenericRepo : Repo
    {
        #region Constractor
        public GenericRepo(IConfiguration Configuration) :
            base(Configuration, "MHC:ConnectionString")
        { }
        #endregion

        #region Protected Methods
        public override async Task<StoredResult>
            Check(string Operation, string Id, long longID = 0)
        {
            var task = Task.Factory.StartNew(() =>
            {
                StoredResult result = new StoredResult();
                if (Operation == "1")
                {
                    result.Success = true;
                    result.LongID = longID;
                    result.ID = Id;
                }
                else if (Operation == "2")
                {
                    result.Success = false;
                    result.ErrorMessage = "The patient already Existed";
                }
                else if (Operation == "3")
                {
                    result.Success = false;
                    result.ErrorMessage = "The Patient already Existed at another Clinic";
                }
                else if (Operation == "4")
                {
                    result.Success = false;
                    result.ErrorMessage = "The relative data is already Existed";
                }
                else if (Operation == "5")
                {
                    result.Success = false;
                    result.ErrorMessage = "The Consent is already existed";
                }
                else if (Operation == "6")
                {
                    result.Success = false;
                    result.ErrorMessage = "Category Already Existed";
                }
                else if (Operation == "7")
                {
                    result.Success = false;
                    result.ErrorMessage = "Investigation type is already existed.";
                }
                else if (Operation == "8")
                {
                    result.Success = false;
                    result.ErrorMessage = "The History is already Existed";
                }
                else if (Operation == "9")
                {
                    result.Success = false;
                    result.ErrorMessage = "The Medicine is already exisited";
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "There is an error ocure while trying to perform your request.please try sgain.";
                }
                result.ErrorCode = Operation;
                return result;
            });
            await task;
            return task.Result;
        }
        #endregion
    }
}
