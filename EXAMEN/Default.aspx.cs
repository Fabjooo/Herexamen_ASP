using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EXAMEN
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdownControls();
            }
        }

        protected void LoadDropdownControls()
        {
            var context = new DatabaseEntities();
            var companyList = context.Company.OrderBy(x => x.name).ToList();
            ddlCompanies.DataSource = companyList;
            ddlCompanies.DataValueField = "Id";
            ddlCompanies.DataTextField = "name";
            ddlCompanies.DataBind();

            var Department2List = context.Company.OrderBy(x => x.name).ToList();
            ddlCompanies2.DataSource = Department2List;
            ddlCompanies2.DataValueField = "Id";
            ddlCompanies2.DataTextField = "name";
            ddlCompanies2.DataBind();
        }

        protected void btnSaveCompany_OnClick(object sender, EventArgs e)
        {
            var context = new DatabaseEntities();
            var companyname = txtCompanyName.Text;
            var companyaddress = txtCompanyAddress.Text;
            var companyheadquarter = txtCompanyHeadquarter.Text;

            var company = new Company();

            company.name = companyname;
            company.address = companyaddress;
            company.headquarter = companyheadquarter;

            context.Company.Add(company);
            context.SaveChanges();

            LoadDropdownControls();
        }

        protected void btnSaveDepartment_OnClick(object sender, EventArgs e)
        {
            var context = new DatabaseEntities();
            var departmentName = txtDepartment.Text;

            var department = new Department();
            department.name = departmentName;

            context.Department.Add(department);
            context.SaveChanges();

            var newDepartment = context.Department.FirstOrDefault(x => x.name == department.name);
            var companyDepartment = new CompanyDepartment();

            companyDepartment.fk_company = Convert.ToInt32(ddlCompanies.SelectedValue);
            companyDepartment.fk_department = newDepartment.Id;
            context.SaveChanges();

            LoadDropdownControls();
        }

        protected void btnSaveEmployee_OnClick(object sender, EventArgs e)
        {
            var context = new DatabaseEntities();
            var employeeFirstName = txtEmployeeFirstName.Text;
            var employeeLastName = txtEmployeeLastName.Text;
            var employeeBirthday = txtEmployeeBirthday.Text;
            var employeeGender = radioGender.Checked;
            var employeeTitle = txtEmployeeTitle.Text;

            var employee = new Employee();

            employee.firstname = employeeFirstName;
            employee.lastname = employeeLastName;
            employee.birthday = Convert.ToDateTime(employeeBirthday);
            employee.gender = employeeGender;
            employee.title = employeeTitle;

            context.Employee.Add(employee);
            context.SaveChanges();

            var newEmployee = context.Employee.SingleOrDefault(x =>
                x.firstname == employee.firstname && x.lastname == employee.lastname &&
                employee.birthday == employee.birthday);

            var departmentEmployee = new DepartmenEmployee();
            departmentEmployee.fk_employee = newEmployee.Id;
            departmentEmployee.fk_department = Convert.ToInt32(ddlDepartmens.SelectedValue);
            context.DepartmenEmployee.Add(departmentEmployee);
            context.SaveChanges();

            LoadDropdownControls();
        }

        protected void ddlCompanies2_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var context = new DatabaseEntities();
            var companyId = Convert.ToInt32(ddlCompanies2.SelectedValue);

            var companydepartments = context.CompanyDepartment.Where(x => x.Company.Id.Equals(companyId)).ToList().Select(x => new{x.Department.Id, x.Department.name});
            
            ddlDepartmens.DataSource = companydepartments;
            ddlDepartmens.DataValueField = "Id";
            ddlDepartmens.DataTextField = "name";
            ddlDepartmens.DataBind();

            UpdatePanel1.Update();
        }
    }
}