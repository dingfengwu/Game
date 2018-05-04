﻿using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Game.Web.Areas.Admin.Models.Customers;
using Game.Base.Domain.Customers;
using Game.Data;
using Game.Services.Customers;
using Game.Services.Localization;
using Game.Facade.Validators;

namespace Game.Web.Areas.Admin.Validators.Customers
{
    public partial class CustomerValidator : BaseGameValidator<CustomerModel>
    {
        public CustomerValidator(ILocalizationService localizationService,
            ICustomerService customerService,
            CustomerSettings customerSettings,
            IDbContext dbContext)
        {
            //ensure that valid email address is entered if Registered role is checked to avoid registered customers with empty email address
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                //.WithMessage("Valid Email is required for customer to be in 'Registered' role")
                .WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"))
                //only for registered users
                .When(x => IsRegisteredCustomerRoleChecked(x, customerService));

            //form fields
            if (customerSettings.CountryEnabled && customerSettings.CountryRequired)
            {
                RuleFor(x => x.CountryId)
                    .NotEqual(0)
                    .WithMessage(localizationService.GetResource("Account.Fields.Country.Required"))
                    //only for registered users
                    .When(x => IsRegisteredCustomerRoleChecked(x, customerService));
            }
            if (customerSettings.CompanyRequired && customerSettings.CompanyEnabled)
            {
                RuleFor(x => x.Company)
                    .NotEmpty()
                    .WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Company.Required"))
                    //only for registered users
                    .When(x => IsRegisteredCustomerRoleChecked(x, customerService));
            }
            if (customerSettings.StreetAddressRequired && customerSettings.StreetAddressEnabled)
            {
                RuleFor(x => x.StreetAddress)
                    .NotEmpty()
                    .WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.StreetAddress.Required"))
                    //only for registered users
                    .When(x => IsRegisteredCustomerRoleChecked(x, customerService));
            }
            if (customerSettings.StreetAddress2Required && customerSettings.StreetAddress2Enabled)
            {
                RuleFor(x => x.StreetAddress2)
                    .NotEmpty()
                    .WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.StreetAddress2.Required"))
                    //only for registered users
                    .When(x => IsRegisteredCustomerRoleChecked(x, customerService));
            }
            if (customerSettings.ZipPostalCodeRequired && customerSettings.ZipPostalCodeEnabled)
            {
                RuleFor(x => x.ZipPostalCode)
                    .NotEmpty()
                    .WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.ZipPostalCode.Required"))
                    //only for registered users
                    .When(x => IsRegisteredCustomerRoleChecked(x, customerService));
            }
            if (customerSettings.CityRequired && customerSettings.CityEnabled)
            {
                RuleFor(x => x.City)
                    .NotEmpty()
                    .WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.City.Required"))
                    //only for registered users
                    .When(x => IsRegisteredCustomerRoleChecked(x, customerService));
            }
            if (customerSettings.PhoneRequired && customerSettings.OtherPhoneEnabled)
            {
                RuleFor(x => x.Phone)
                    .NotEmpty()
                    .WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Phone.Required"))
                    //only for registered users
                    .When(x => IsRegisteredCustomerRoleChecked(x, customerService));
            }
            if (customerSettings.FaxRequired && customerSettings.FaxEnabled)
            {
                RuleFor(x => x.Fax)
                    .NotEmpty()
                    .WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Fax.Required"))
                    //only for registered users
                    .When(x => IsRegisteredCustomerRoleChecked(x, customerService));
            }

            //TODO:未对数据库字段信息进行验证
            //SetDatabaseValidationRules<Customer>(dbContext); 
        }

        private bool IsRegisteredCustomerRoleChecked(CustomerModel model, ICustomerService customerService)
        {
            var allCustomerRoles = customerService.GetAllCustomerRoles(true);
            var newCustomerRoles = new List<CustomerRole>();
            foreach (var customerRole in allCustomerRoles)
                if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                    newCustomerRoles.Add(customerRole);

            var isInRegisteredRole = newCustomerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Registered) != null;
            return isInRegisteredRole;
        }
    }
}