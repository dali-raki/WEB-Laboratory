using GLAB.Domains.Models.Laboratories;
using GLAB.App.Laboratories;
using GLAB.Domains.Models.Laboratories;
using GLAB.Domains.Shared;
using GLAB.Infra.Storages.LaboratoriesStorages;
using System.Transactions;
using Email.App;
using GLAB.App.Users;
using GLAB.Domains.Models.Users;
using Users.Domains.Users;

namespace GLAB.Implementation.Services.Laboratories
{
    public class LabService : ILabService

    {
        private readonly ILaboratoryStorage labStorage;
        private readonly IEmailService emailService;
        private readonly IUserService userService;

        public LabService(ILaboratoryStorage labStorage,IEmailService _emailService,IUserService userService)
        {
            this.labStorage = labStorage;
            emailService = _emailService;
            this.userService = userService;
        }


        public async Task<Result> CreateLaboratory(Laboratory laboratory)

        {

          // FIX THIS exeption using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))

          

                try

                {   
                    
                    List<ErrorCode> errorList = validateLaboratoireForInsert(laboratory); // Corrected method name

                    if (errorList.Any())

                        return Result.Failure(errorList);

                    bool labExistsbyId = await labStorage.LaboratoryExistsById(laboratory.LaboratoryId);
                    Laboratory labExistsbyEmail = await labStorage.SelectLaboratoryByEmail(laboratory.Email);
                    bool labExistsbyAcronyme = await labStorage.LaboratoryExistsByAcronyme(laboratory.Acronyme);
                    bool labExistsbyAgrement = await labStorage.LaboratoryExistsByAgrementNum(laboratory.NumAgrement);




                errorList.Clear();

                if (labExistsbyId)
                    errorList.Add(LaboratoryErrors.LaboratoryIdExists);

                if (labExistsbyAcronyme)
                    errorList.Add(LaboratoryErrors.LaboratoryAcronymExists);

                if (labExistsbyAgrement)
                    errorList.Add(LaboratoryErrors.LaboratoryAgrementExists);


                if (labExistsbyEmail != null)
                    errorList.Add(LaboratoryErrors.LaboratoryEmailExists);


                if (errorList.Any())

                    return Result.Failure(errorList);

                await labStorage.InsertLaboratory(laboratory);
                    
                    

                    await userService.CreateAdmin(laboratory);


                   // scope.Complete();
                    return Result.Succes;

                }

                catch (Exception ex)

                {

                    // Handle the exception if needed

                    Console.WriteLine($"Error setting laboratory: {ex.Message}");

                    return Result.Failure(new[] { "An error occurred while setting the laboratory." });
                        
                }

            

        }


        private List<ErrorCode> validateLaboratoireForInsert(Laboratory laboratoire)

        {

            List<ErrorCode> errors = new List<ErrorCode>();



            if (string.IsNullOrWhiteSpace(laboratoire.Name))

                errors.Add(LaboratoryErrors.NameEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Acronyme))

                errors.Add(LaboratoryErrors.AcronymeEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Adresse))

                errors.Add(LaboratoryErrors.AddressEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.University))

                errors.Add(LaboratoryErrors.UniversityEmpty);

          /*  if (laboratoire.AgrementDate)
            {
                errors.Add(LaboratoryErrors.CreationDateInvalid);
            }*/


            if (string.IsNullOrWhiteSpace(laboratoire.PhoneNumber))

                errors.Add(LaboratoryErrors.PhoneNumberEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Email))

                errors.Add(LaboratoryErrors.EmailEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.NumAgrement))

                errors.Add(LaboratoryErrors.AgreementNumberEmpty);

            if(string.IsNullOrWhiteSpace(laboratoire.DirectorId))

                errors.Add(LaboratoryErrors.IdEmpty);
            

            if (string.IsNullOrWhiteSpace(laboratoire.WebSite))

                errors.Add(LaboratoryErrors.WebSiteEmpty);



            return errors;

        }


        public async Task<List<Laboratory>> GetLaboratories()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                try
                {
                    List<Laboratory> laboratories = await labStorage.SelectLaboratories();


                    if (laboratories == null)
                    {
                        throw new Exception("Laboratories list is null.");
                    }
                    scope.Complete();
                    return laboratories;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting laboratories: {ex.Message}");
                    throw;
                }
        }



        public async Task<Result> SetLaboratory(Laboratory laboratoire)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                   

                    await labStorage.UpdateLaboratory(laboratoire);
                    scope.Complete();
                    return Result.Succes;
                }
                catch (Exception ex)
                {
                    // Handle the exception if needed
                    Console.WriteLine($"Error setting laboratory: {ex.Message}");
                    return Result.Failure(new [] { "An error occurred while setting the laboratory." });
                }
            }
        }




        private List<ErrorCode> validateLaboratoireForUpdate(Laboratory laboratoire)
        {
            List<ErrorCode> errors = new List<ErrorCode>();


            if (string.IsNullOrWhiteSpace(laboratoire.Name))

                errors.Add(LaboratoryErrors.NameEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Acronyme))

                errors.Add(LaboratoryErrors.AcronymeEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Adresse))

                errors.Add(LaboratoryErrors.AddressEmpty);


            if (string.IsNullOrWhiteSpace(laboratoire.University))

                errors.Add(LaboratoryErrors.UniversityEmpty);

/*
            if (!DateTime.TryParse(laboratoire.AgrementDate, out _))
            {
                errors.Add(LaboratoryErrors.CreationDateInvalid);
            }*/


            if (string.IsNullOrWhiteSpace(laboratoire.PhoneNumber))

                errors.Add(LaboratoryErrors.PhoneNumberEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Email))

                errors.Add(LaboratoryErrors.EmailEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.NumAgrement))

                errors.Add(LaboratoryErrors.AgreementNumberEmpty);

            if(string.IsNullOrWhiteSpace(laboratoire.DirectorId))

                errors.Add(LaboratoryErrors.IdEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.WebSite))

                errors.Add(LaboratoryErrors.WebSiteEmpty);

            return errors;
        }


        public async Task<Result> SetLaboratoryStatus(string id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    await labStorage.UpdateLaboratoryStatus(id);

                    scope.Complete();
                    return Result.Succes;
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a failure result with the exception message
                return Result.Failure(new[] { ex.Message });
            }
        }

        public async Task<Laboratory> GetLaboratoryById(string id)
        {
            bool labExists = await labStorage.LaboratoryExistsById(id);
            if (!labExists)
            {
                // Handle the case where the laboratory with the specified ID does not exist
                throw new ArgumentException("Laboratory with the specified ID does not exist.");
            }
            return await labStorage.SelectLaboratoryById(id);
        }

        public async  Task<Laboratory> GetLaboratoryByName(string name)
        {
            bool labExists = await labStorage.LaboratoryExistsByName(name);
            if (!labExists)
            {
                // Handle the case where the laboratory with the specified name does not exist
                throw new ArgumentException("Laboratory with the specified name does not exist.");
            }
            return await labStorage.SelectLaboratoryByName(name);
        }

        public async Task<Laboratory?> GetLaboratoryInformations(string email)
        {  
          return await labStorage.SelectLaboratoryInformations(email);
        }

        public async Task<Laboratory> GetLaboratoryByUser(string userId)
        {
            try
            {
                return await labStorage.SelectLaboratoryByUser(userId);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }





    }


 



}