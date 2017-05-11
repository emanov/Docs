﻿using System.Threading.Tasks;
using ContactManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace ContactManager.Authorization
{
    public class ContactIsOwnerAuthorizationHandler 
                : AuthorizationHandler<OperationAuthorizationRequirement, Contact>
    {
        UserManager<ApplicationUser> _userManager;
        
        public ContactIsOwnerAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;  
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
                                             OperationAuthorizationRequirement requirement, 
                                             Contact resource)
        {
            if (context.User == null)
            {
                return Task.FromResult(0);
            }

            if (resource == null)
            {
                return Task.FromResult(0);
            }

            // Normal users cannot change approve or reject.
            // Only managers and admins can approve or reject.
            if (requirement.Name != Constants.ApproveOperationName && 
               requirement.Name != Constants.RejectOperationName)
            {
                if (resource.OwnerID == _userManager.GetUserId(context.User))
                {
                    context.Succeed(requirement);
                }
            }
            
            return Task.FromResult(0);
        }
    }
}
