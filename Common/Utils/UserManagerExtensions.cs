﻿using Common.Constants;
using Common.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Common.Utils
{
    public static class UserManagerExtensions
    {
        public static bool IsUserInMedEmployeeRole(this UserManager<AppUser> userManager, string username)
        {
            var user = userManager.FindByNameAsync(username).Result;
            var isMedEmployee = userManager.IsInRoleAsync(user, RolesConstants.Doctor).Result
                || userManager.IsInRoleAsync(user, RolesConstants.Nurse).Result;
            return isMedEmployee;
        }

        public static bool IsUserInPatientRole(this UserManager<AppUser> userManager, string username)
        {
            var user = userManager.FindByNameAsync(username).Result;
            var isPatient = userManager.IsInRoleAsync(user, RolesConstants.Patient).Result;
            return isPatient;
        }

        public static bool IsUserInAdminRole(this UserManager<AppUser> userManager, string username)
        {
            var user = userManager.FindByNameAsync(username).Result;
            var isAdmin = userManager.IsInRoleAsync(user, RolesConstants.Administrator).Result;
            return isAdmin;
        }
    }
}
