﻿using Common.Constants;
using Common.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Entities;
using Common.Entities.Identity;

namespace DataLayer.DbContext
{
    public static class WebHostDbExtensions
    {
        public static IWebHost Seed(this IWebHost webhost)
        {
            using (var scope = webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                // alternatively resolve UserManager instead and pass that if only think you want to seed are the users
                using (var dbInit = scope.ServiceProvider.GetRequiredService<IDbInitializer>())
                {
                    dbInit.InitializeAsync().GetAwaiter().GetResult();
                }
            }
            return webhost;
        }
    }

    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            AppDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            await FillRolesAsync();
            await FillRolesClaimsAsync();
            await FillUsersAsync();
        }

        private async Task FillRolesAsync()
        {
            if (!_dbContext.Roles.Any())
            {
                // TODO: add claims to roles
                await _roleManager.CreateAsync(new IdentityRole(RolesConstants.Administrator));
                await _roleManager.CreateAsync(new IdentityRole(RolesConstants.Patient));
                await _roleManager.CreateAsync(new IdentityRole(RolesConstants.MedicalEmployee));
            }
        }

        private async Task FillRolesClaimsAsync()
        {
            if (!_dbContext.RoleClaims.Any())
            {
                var viewDonorRequestList = new Claim("View Donor Request List", ((int)AppClaimTypes.ViewDonorRequestList).ToString());
                var viewDonorRequest = new Claim("View Donor Request", ((int)AppClaimTypes.ViewDonorRequest).ToString());
                var editDonorRequest = new Claim("Edit Donor Request", ((int)AppClaimTypes.EditDonorRequest).ToString());
                var changeDonorRequestStatus = new Claim("Change Donor Request Status", ((int)AppClaimTypes.ChangeDonorRequestStatus).ToString());

                var viewOrganRequestList = new Claim("View Organ Request List", ((int)AppClaimTypes.ViewOrganRequestList).ToString());
                var viewOrganRequest = new Claim("View Organ Request", ((int)AppClaimTypes.ViewOrganRequest).ToString());
                var editOrganRequest = new Claim("Edit Organ Request", ((int)AppClaimTypes.EditOrganRequest).ToString());
                var createOrganRequest = new Claim("Create Organ Request", ((int)AppClaimTypes.CreateOrganRequest).ToString());

                var connectDonorAndOrganRequests = new Claim("Connect Donor And Organ Requests", ((int)AppClaimTypes.ConnectDonorAndOrganRequests).ToString());

                var addNewOrgan = new Claim("Add New Organ", ((int)AppClaimTypes.AddNewOrgan).ToString());
                var addNewMedicalEmployee = new Claim("Add New Medical Employee", ((int)AppClaimTypes.AddNewMedicalEmployee).ToString());

                var addOrganInfo = new Claim("Add Organ Info", ((int)AppClaimTypes.AddOrganInfo).ToString());
                var registerNewPatient = new Claim("Register New Patient", ((int)AppClaimTypes.RegisterNewPatient).ToString());
                var addClinic = new Claim("Add Clinic", ((int)AppClaimTypes.AddClinic).ToString());

                var viewOrganTransportData = new Claim("View Organ Transport Data", ((int)AppClaimTypes.ViewOrganTransportData).ToString());

                var adminRole = await _roleManager.FindByNameAsync(RolesConstants.Administrator);
                await _roleManager.AddClaimAsync(adminRole, viewDonorRequestList);
                await _roleManager.AddClaimAsync(adminRole, viewDonorRequest);
                await _roleManager.AddClaimAsync(adminRole, editDonorRequest);
                await _roleManager.AddClaimAsync(adminRole, changeDonorRequestStatus);
                await _roleManager.AddClaimAsync(adminRole, viewOrganRequestList);
                await _roleManager.AddClaimAsync(adminRole, viewOrganRequest);
                await _roleManager.AddClaimAsync(adminRole, editOrganRequest);
                await _roleManager.AddClaimAsync(adminRole, createOrganRequest);
                await _roleManager.AddClaimAsync(adminRole, connectDonorAndOrganRequests);
                await _roleManager.AddClaimAsync(adminRole, addNewOrgan);
                await _roleManager.AddClaimAsync(adminRole, addNewMedicalEmployee);
                await _roleManager.AddClaimAsync(adminRole, addOrganInfo);
                await _roleManager.AddClaimAsync(adminRole, registerNewPatient);
                await _roleManager.AddClaimAsync(adminRole, addClinic);


                var patientRole = await _roleManager.FindByNameAsync(RolesConstants.Patient);
                await _roleManager.AddClaimAsync(patientRole, viewOrganRequest);
                await _roleManager.AddClaimAsync(patientRole, viewOrganTransportData);

                var medEmployeeRole = await _roleManager.FindByNameAsync(RolesConstants.MedicalEmployee);
                await _roleManager.AddClaimAsync(medEmployeeRole, viewDonorRequestList);
                await _roleManager.AddClaimAsync(medEmployeeRole, viewDonorRequest);
                await _roleManager.AddClaimAsync(medEmployeeRole, editDonorRequest);
                await _roleManager.AddClaimAsync(medEmployeeRole, changeDonorRequestStatus);

                await _roleManager.AddClaimAsync(medEmployeeRole, viewOrganRequestList);
                await _roleManager.AddClaimAsync(medEmployeeRole, viewOrganRequest);
                await _roleManager.AddClaimAsync(medEmployeeRole, editDonorRequest);
                await _roleManager.AddClaimAsync(medEmployeeRole, createOrganRequest);

                await _roleManager.AddClaimAsync(medEmployeeRole, connectDonorAndOrganRequests);

                await _roleManager.AddClaimAsync(medEmployeeRole, addNewOrgan);
                await _roleManager.AddClaimAsync(medEmployeeRole, registerNewPatient);
                await _roleManager.AddClaimAsync(medEmployeeRole, viewOrganTransportData);
            }
        }

        private async Task FillUsersAsync()
        {
            if (_dbContext.Users.Any())
            {
                return;
            }

            //Create the default Admin account and apply the Administrator role
            string password = "Test123!";

            DateTime created = DateTime.UtcNow;
            string createdBy = "Seed";

            string adminEmail = "admin1@test.com";
            string patientEmail = "patient1@test.com";
            string medEmployeeEmail = "medEmployee1@test.com";

            UserInfo adminUserInfo = new UserInfo
            {
                Email = adminEmail,
                Created = created,
                CreatedBy = createdBy
            };
            var res = await _userManager.CreateAsync(new AppUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true, UserInfo = adminUserInfo }, password);
            res = await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(adminEmail), RolesConstants.Administrator);

            UserInfo patientUserInfo = new UserInfo
            {
                Email = patientEmail,
                ZipCode = "60000",
                Country = "Ukraine",
                City = "Kharkiv",
                FirstName = "First name 1",
                SecondName = "Second name 1",
                BirthDate = DateTime.UtcNow.AddYears(-50),
                Notes = "Test Info",
                AddressLine1 = "Test address line 1 1",
                AddressLine2 = "Test address line 2 1",
                PhoneNumber = "+380991234567",
                Created = created,
                CreatedBy = createdBy
            };
            res = await _userManager.CreateAsync(new AppUser { UserName = patientEmail, Email = patientEmail, EmailConfirmed = true, UserInfo = patientUserInfo }, password);
            res = await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(patientEmail), RolesConstants.Patient);

            UserInfo medEmployeeUserInfo = new UserInfo
            {
                Email = medEmployeeEmail,
                ZipCode = "60000",
                Country = "Ukraine",
                City = "Kharkiv",
                FirstName = "First name 1 3",
                SecondName = "Second name 1 3",
                BirthDate = DateTime.UtcNow.AddYears(-30),
                Notes = "Test Info",
                AddressLine1 = "Test address line 1 3",
                AddressLine2 = "Test address line 2 3",
                PhoneNumber = "+380991245628",
                Created = created,
                CreatedBy = createdBy
            };
            res = await _userManager.CreateAsync(new AppUser { UserName = medEmployeeEmail, Email = medEmployeeEmail, EmailConfirmed = true, UserInfo = medEmployeeUserInfo }, password);
            res = await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(medEmployeeEmail), RolesConstants.MedicalEmployee);
        }

        public void Dispose()
        {
            this._dbContext?.Dispose();
            this._userManager?.Dispose();
            this._roleManager?.Dispose();
        }
    }
}
