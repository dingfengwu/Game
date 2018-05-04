using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Base;
using Game.Base.Domain.Common;
using Game.Base.Domain.Customers;
using Game.Base.Infrastructure;
using Game.Services.Common;
using Game.Services.Customers;
using Game.Services.Helpers;
using Game.Services.Localization;
using Game.Services.Security;
using Game.Web.Areas.Admin.Models;
using Game.Web.Areas.Admin.Models.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Game.Web.Areas.Admin.Controllers
{
    public class CustomerController : BaseAdminController
    {
        #region 字段

        readonly ICustomerService _customerService;
        readonly IPermissionService _permissionService;
        readonly IDateTimeHelper _dateTimeHelper;
        readonly CustomerSettings _customerSettings;
        readonly ILocalizationService _localizationService;
        readonly ICustomerAttributeService _customerAttributeService;
        readonly ICustomerAttributeParser _customerAttributeParser;
        readonly IGenericAttributeService _genericAttributeService;

        #endregion

        #region 构造函数

        public CustomerController(ICustomerService customerService,
            IPermissionService permissionService,
            IDateTimeHelper dateTimeHelper,
            CustomerSettings customerSettings,
            ILocalizationService localizationService,
            ICustomerAttributeService customerAttributeService,
            ICustomerAttributeParser customerAttributeParser,
            IGenericAttributeService genericAttributeService)
        {
            this._customerService = customerService;
            this._permissionService = permissionService;
            this._dateTimeHelper = dateTimeHelper;
            this._customerSettings = customerSettings;
            this._localizationService = localizationService;
            this._customerAttributeService = customerAttributeService;
            this._customerAttributeParser = customerAttributeParser;
            this._genericAttributeService = genericAttributeService;
        }

        #endregion

        #region Actions

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customers = _customerService.GetAllCustomers(customerRoleIds: new[] { 3 });
            return View(customers);
        }

        public IActionResult Edit(int id=0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(id);
            if (customer == null || customer.Deleted)
                return RedirectToAction("List");

            var model = new CustomerModel();
            PrepareCustomerModel(model, customer, false);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(CustomerModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null || customer.Deleted)
                //No customer found with the specified id
                return RedirectToAction("List");

            //TODO:要对客户数据进行校验

            if (ModelState.IsValid)
            {
                //TODO:要对客户业务数据进行校验

                //基本信息
                customer.Username = model.Username;
                customer.PhoneNumber = model.PhoneNumber;
                customer.Email = model.Email;
                //扩展信息
                _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.FirstName, model.FirstName);
                _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.LastName, model.LastName);
                _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StreetAddress, model.StreetAddress);
                //保存
                _customerService.UpdateCustomer(customer);

                return RedirectToAction("List");
            }

            PrepareCustomerModel(model, customer, true);
            return View(model);
        }

        #endregion
        
        #region 实用方法

        protected virtual void PrepareCustomerModel(CustomerModel model, Customer customer, bool excludeProperties)
        {
            if (customer != null)
            {
                model.Id = customer.Id;
                if (!excludeProperties)
                {
                    model.Email = customer.Email;
                    model.Username = customer.Username;
                    model.AdminComment = customer.AdminComment;
                    model.Active = customer.Active;
                    model.PhoneNumber = customer.PhoneNumber;

                    model.CreatedOn = _dateTimeHelper.ConvertToUserTime(customer.CreatedOnUtc, DateTimeKind.Utc);
                    model.LastActivityDate = _dateTimeHelper.ConvertToUserTime(customer.LastActivityDateUtc, DateTimeKind.Utc);
                    model.LastIpAddress = customer.LastIpAddress;
                    model.LastVisitedPage = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastVisitedPage);

                    model.SelectedCustomerRoleIds = customer.CustomerCustomerRoleMapping.Select(cr => cr.CustomerRole.Id).ToList();
                    
                    //form fields
                    model.FirstName = customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName);
                    model.LastName = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName);
                    model.Gender = customer.GetAttribute<string>(SystemCustomerAttributeNames.Gender);
                    model.DateOfBirth = customer.GetAttribute<DateTime?>(SystemCustomerAttributeNames.DateOfBirth);
                    model.Company = customer.GetAttribute<string>(SystemCustomerAttributeNames.Company);
                    model.StreetAddress = customer.GetAttribute<string>(SystemCustomerAttributeNames.StreetAddress);
                    model.StreetAddress2 = customer.GetAttribute<string>(SystemCustomerAttributeNames.StreetAddress2);
                    model.ZipPostalCode = customer.GetAttribute<string>(SystemCustomerAttributeNames.ZipPostalCode);
                    model.City = customer.GetAttribute<string>(SystemCustomerAttributeNames.City);
                    model.CountryId = customer.GetAttribute<int>(SystemCustomerAttributeNames.CountryId);
                    model.StateProvinceId = customer.GetAttribute<int>(SystemCustomerAttributeNames.StateProvinceId);
                    model.Phone = customer.GetAttribute<string>(SystemCustomerAttributeNames.Phone);
                    model.Fax = customer.GetAttribute<string>(SystemCustomerAttributeNames.Fax);
                }
            }

            //customer attributes
            PrepareCustomerAttributeModel(model, customer);

            model.GenderEnabled = _customerSettings.GenderEnabled;
            model.DateOfBirthEnabled = _customerSettings.DateOfBirthEnabled;
            model.CompanyEnabled = _customerSettings.CompanyEnabled;
            model.StreetAddressEnabled = _customerSettings.StreetAddressEnabled;
            model.StreetAddress2Enabled = _customerSettings.StreetAddress2Enabled;
            model.ZipPostalCodeEnabled = _customerSettings.ZipPostalCodeEnabled;
            model.CityEnabled = _customerSettings.CityEnabled;
            model.CountryEnabled = _customerSettings.CountryEnabled;
            model.StateProvinceEnabled = _customerSettings.StateProvinceEnabled;
            model.OtherPhoneEnabled = _customerSettings.OtherPhoneEnabled;
            model.FaxEnabled = _customerSettings.FaxEnabled;
            
            //customer roles
            var allRoles = _customerService.GetAllCustomerRoles(true);
            var adminRole = allRoles.FirstOrDefault(c => c.SystemName == SystemCustomerRoleNames.Registered);
            //precheck Registered Role as a default role while creating a new customer through admin
            if (customer == null && adminRole != null)
            {
                model.SelectedCustomerRoleIds.Add(adminRole.Id);
            }
            foreach (var role in allRoles)
            {
                model.AvailableCustomerRoles.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Id.ToString(),
                    Selected = model.SelectedCustomerRoleIds.Contains(role.Id)
                });
            }
            
            //sending of the welcome message:
            //1. "admin approval" registration method
            //2. already created customer
            //3. registered
            model.AllowSendingOfWelcomeMessage = _customerSettings.UserRegistrationType == UserRegistrationType.AdminApproval &&
                customer != null &&
                customer.IsRegistered();
            //sending of the activation message
            //1. "email validation" registration method
            //2. already created customer
            //3. registered
            //4. not active
            model.AllowReSendingOfActivationMessage = _customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation &&
                customer != null &&
                customer.IsRegistered() &&
                !customer.Active;
        }

        protected virtual void PrepareCustomerAttributeModel(CustomerModel model, Customer customer)
        {
            var customerAttributes = _customerAttributeService.GetAllCustomerAttributes();
            foreach (var attribute in customerAttributes)
            {
                var attributeModel = new CustomerModel.CustomerAttributeModel
                {
                    Id = attribute.Id,
                    Name = attribute.Name,
                    IsRequired = attribute.IsRequired,
                    AttributeControlType = attribute.AttributeControlType,
                };

                if (attribute.ShouldHaveValues())
                {
                    //values
                    var attributeValues = _customerAttributeService.GetCustomerAttributeValues(attribute.Id);
                    foreach (var attributeValue in attributeValues)
                    {
                        var attributeValueModel = new CustomerModel.CustomerAttributeValueModel
                        {
                            Id = attributeValue.Id,
                            Name = attributeValue.Name,
                            IsPreSelected = attributeValue.IsPreSelected
                        };
                        attributeModel.Values.Add(attributeValueModel);
                    }
                }


                //set already selected attributes
                if (customer != null)
                {
                    var selectedCustomerAttributes = customer.GetAttribute<string>(SystemCustomerAttributeNames.CustomCustomerAttributes, _genericAttributeService);
                    switch (attribute.AttributeControlType)
                    {
                        case AttributeControlType.DropdownList:
                        case AttributeControlType.RadioList:
                        case AttributeControlType.Checkboxes:
                            {
                                if (!string.IsNullOrEmpty(selectedCustomerAttributes))
                                {
                                    //clear default selection
                                    foreach (var item in attributeModel.Values)
                                        item.IsPreSelected = false;

                                    //select new values
                                    var selectedValues = _customerAttributeParser.ParseCustomerAttributeValues(selectedCustomerAttributes);
                                    foreach (var attributeValue in selectedValues)
                                        foreach (var item in attributeModel.Values)
                                            if (attributeValue.Id == item.Id)
                                                item.IsPreSelected = true;
                                }
                            }
                            break;
                        case AttributeControlType.ReadonlyCheckboxes:
                            {
                                //do nothing
                                //values are already pre-set
                            }
                            break;
                        case AttributeControlType.TextBox:
                        case AttributeControlType.MultilineTextbox:
                            {
                                if (!string.IsNullOrEmpty(selectedCustomerAttributes))
                                {
                                    var enteredText = _customerAttributeParser.ParseValues(selectedCustomerAttributes, attribute.Id);
                                    if (enteredText.Any())
                                        attributeModel.DefaultValue = enteredText[0];
                                }
                            }
                            break;
                        case AttributeControlType.Datepicker:
                        case AttributeControlType.ColorSquares:
                        case AttributeControlType.ImageSquares:
                        case AttributeControlType.FileUpload:
                        default:
                            //not supported attribute control types
                            break;
                    }
                }

                model.CustomerAttributes.Add(attributeModel);
            }
        }

        #endregion
    }
}