using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.EntitySql;
using System.Data.Entity.Migrations;
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

            ddlCompanyLists.DataSource = companyList;
            ddlCompanyLists.DataValueField = "Id";
            ddlCompanyLists.DataTextField = "name";
            ddlCompanyLists.DataBind();
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

            var newDepartment = context.Department.FirstOrDefault(x => x.Id == department.Id);
            var companyDepartment = new CompanyDepartment();

            companyDepartment.fk_company = Convert.ToInt32(ddlCompanies.SelectedValue);
            companyDepartment.fk_department = newDepartment.Id;
            context.CompanyDepartment.Add(companyDepartment);
            context.SaveChanges();

            LoadDropdownControls();
            upCompanyDetails.Update();
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

        protected void ddlCompanyLists_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var context = new DatabaseEntities();
            var companyId = Convert.ToInt32(ddlCompanyLists.SelectedValue);

            var companydepartments = context.CompanyDepartment.Where(x => x.fk_company == companyId).OrderBy(x => x.Company.name).ToList();
            rpCompany.DataSource = companydepartments;
            rpCompany.DataBind();
            upCompanyDetails.Update();
        }

        protected void lnkbtnEdit_OnClick(object sender, EventArgs e)
        {
            var context = new DatabaseEntities();
            var btn = (LinkButton)sender;
            Session["departmentid"] = Convert.ToString(int.Parse(btn.CommandArgument));
            var departmentvalue = Convert.ToInt32(Session["departmentid"]);
            var department = context.Department.FirstOrDefault(x => x.Id == departmentvalue);
            txtEditDepartment.Text = department.name;
            upEditDepartmentText.Update();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$(function() { $('#EditDepartmentModal').modal('show'); });</script>", false);
        }

        protected void btnSaveEditedDepartment_OnClick(object sender, EventArgs e)
        {
            var context = new DatabaseEntities();
            var departmentid = Convert.ToInt32(Session["departmentid"]);
            var department = context.Department.FirstOrDefault(x => x.Id == departmentid);
            department.name = txtEditDepartment.Text;
            context.SaveChanges();
            upCompanyDetails.Update();
        }

        protected void lnkbtnDelete_OnClick(object sender, EventArgs e)
        {
            var context = new DatabaseEntities();
            var btn = (LinkButton)sender;
            var departmentId = int.Parse(btn.CommandArgument);

            //delete foreign key --> employees within department
            var departmentEmployee = context.DepartmenEmployee.Where(x => x.fk_department == departmentId).ToList();
            if(departmentEmployee != null)
            foreach (var dpe in departmentEmployee)
            {
                context.DepartmenEmployee.Remove(dpe);
                context.SaveChanges();
                }
            //delete the department which is linked to a company
            var companyDepartment = context.CompanyDepartment.FirstOrDefault(x => x.fk_department == departmentId);
            if(companyDepartment != null)
            context.CompanyDepartment.Remove(companyDepartment);
            context.SaveChanges();

            //delete actual department
            var department = context.Department.FirstOrDefault(x => x.Id == departmentId);
            if (department != null)
            {
                context.Department.Remove(department);
                context.SaveChanges();
            }
            
        }
    }
}