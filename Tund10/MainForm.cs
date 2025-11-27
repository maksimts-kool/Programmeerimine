using Tund10.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Tund10;

public partial class MainForm : Form
{
    private readonly AutoDbContext _db = new();

    public MainForm()
    {

        InitializeComponent();
        LoadOwners();
        LoadCars();
        LoadServices();
        LoadCarServices();
    }

    void LoadOwners()
    {
        ownersGrid.DataSource = _db.Owners
            .Select(o => new
            {
                o.Id,
                o.FullName,
                o.Phone
            })
            .ToList();
    }

    void LoadCars()
    {
        carsGrid.DataSource = _db.Cars
            .Include(c => c.Owner)
            .Select(c => new
            {
                c.Id,
                c.Brand,
                c.Model,
                c.RegistrationNumber,
                c.OwnerId,
                OwnerName = c.Owner.FullName
            })
            .ToList();

        ownerCombo.DataSource = _db.Owners.ToList();
        ownerCombo.DisplayMember = "FullName";
        ownerCombo.ValueMember = "Id";
    }

    void LoadServices()
    {
        servicesGrid.DataSource = _db.Services
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Price
            })
            .ToList();
    }

    void LoadCarServices()
    {
        carServiceGrid.DataSource = _db.CarServices
            .Include(cs => cs.Car)
            .Include(cs => cs.Service)
            .Select(cs => new
            {
                cs.CarId,
                Car = cs.Car.RegistrationNumber,
                cs.ServiceId,
                Service = cs.Service.Name,
                cs.Mileage,
                cs.DateOfService
            })
            .ToList();

        carSelect.DataSource = null;
        carSelect.DataSource = _db.Cars.ToList();
        carSelect.DisplayMember = "RegistrationNumber";
        carSelect.ValueMember = "Id";

        serviceSelect.DataSource = null;
        serviceSelect.DataSource = _db.Services.ToList();
        serviceSelect.DisplayMember = "Name";
        serviceSelect.ValueMember = "Id";
    }

    void addOwnerBtn_Click(object sender, EventArgs e)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(ownerName.Text) ||
            string.IsNullOrWhiteSpace(ownerPhone.Text))
        {
            MessageBox.Show("Please fill in both Full Name and Phone.");
            return;
        }

        _db.Owners.Add(new Owner
        {
            FullName = ownerName.Text.Trim(),
            Phone = ownerPhone.Text.Trim()
        });
        _db.SaveChanges();
        LoadOwners();
        LoadCars();
        MessageBox.Show("Owner added successfully!");
    }

    void updateOwnerBtn_Click(object sender, EventArgs e)
    {
        if (ownersGrid.CurrentRow == null)
            return;

        int id = (int)ownersGrid.CurrentRow.Cells["Id"].Value;
        var owner = _db.Owners.Find(id);
        if (owner == null)
            return;

        bool changed = false;

        if (!string.IsNullOrWhiteSpace(ownerName.Text) &&
            ownerName.Text != owner.FullName)
        {
            owner.FullName = ownerName.Text;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(ownerPhone.Text) &&
            ownerPhone.Text != owner.Phone)
        {
            owner.Phone = ownerPhone.Text;
            changed = true;
        }

        if (changed)
        {
            _db.SaveChanges();
            LoadOwners();
            LoadCars();
            MessageBox.Show("Owner info updated successfully.");
        }
        else
        {
            MessageBox.Show("Nothing to change.");
        }
    }

    void deleteOwnerBtn_Click(object sender, EventArgs e)
    {
        if (ownersGrid.CurrentRow == null)
            return;

        int id = (int)ownersGrid.CurrentRow.Cells["Id"].Value;

        var owner = _db.Owners
            .Include(o => o.Cars)
            .FirstOrDefault(o => o.Id == id);

        if (owner == null)
            return;

        if (owner.Cars.Any())
        {
            MessageBox.Show("Cannot delete this owner. Remove related cars first.");
            return;
        }

        _db.Owners.Remove(owner);
        _db.SaveChanges();

        LoadOwners();
        LoadCars();
        MessageBox.Show("Owner deleted successfully.");
    }

    void addCarBtn_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(carBrand.Text) ||
            string.IsNullOrWhiteSpace(carModel.Text) ||
            string.IsNullOrWhiteSpace(carReg.Text) ||
            ownerCombo.SelectedValue == null)
        {
            MessageBox.Show("Please fill in all car fields and select an owner.");
            return;
        }

        _db.Cars.Add(new Car
        {
            Brand = carBrand.Text.Trim(),
            Model = carModel.Text.Trim(),
            RegistrationNumber = carReg.Text.Trim(),
            OwnerId = (int)ownerCombo.SelectedValue
        });
        _db.SaveChanges();
        LoadCars();
        LoadCarServices();
        MessageBox.Show("Car added successfully!");
    }

    void updateCarBtn_Click(object sender, EventArgs e)
    {
        if (carsGrid.CurrentRow == null)
            return;

        int id = (int)carsGrid.CurrentRow.Cells["Id"].Value;
        var car = _db.Cars.Find(id);
        if (car == null)
            return;

        bool changed = false;

        if (!string.IsNullOrWhiteSpace(carBrand.Text) && car.Brand != carBrand.Text)
        {
            car.Brand = carBrand.Text;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(carModel.Text) && car.Model != carModel.Text)
        {
            car.Model = carModel.Text;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(carReg.Text) && car.RegistrationNumber != carReg.Text)
        {
            car.RegistrationNumber = carReg.Text;
            changed = true;
        }

        int selectedOwnerId = (int)ownerCombo.SelectedValue;
        if (car.OwnerId != selectedOwnerId)
        {
            car.OwnerId = selectedOwnerId;
            changed = true;
        }

        if (changed)
        {
            _db.SaveChanges();
            LoadCars();
            LoadCarServices();
            MessageBox.Show("Car updated successfully.");
        }
        else
        {
            MessageBox.Show("Nothing to change.");
        }
    }

    void deleteCarBtn_Click(object sender, EventArgs e)
    {
        if (carsGrid.CurrentRow == null)
            return;

        int id = (int)carsGrid.CurrentRow.Cells["Id"].Value;

        var car = _db.Cars
            .Include(c => c.CarServices)
            .FirstOrDefault(c => c.Id == id);

        if (car == null)
            return;

        if (car.CarServices.Any())
        {
            MessageBox.Show("Cannot delete this car. Remove related service records first.");
            return;
        }

        _db.Cars.Remove(car);
        _db.SaveChanges();

        LoadCars();
        LoadCarServices();
        MessageBox.Show("Car deleted successfully.");
    }

    void addServiceBtn_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(serviceName.Text) ||
            string.IsNullOrWhiteSpace(servicePrice.Text))
        {
            MessageBox.Show("Please fill in both Service Name and Price.");
            return;
        }

        if (!decimal.TryParse(servicePrice.Text, out decimal price) || price <= 0)
        {
            MessageBox.Show("Please enter a valid positive price.");
            return;
        }

        _db.Services.Add(new Service
        {
            Name = serviceName.Text.Trim(),
            Price = price
        });
        _db.SaveChanges();
        LoadServices();
        LoadCarServices();
        MessageBox.Show("Service added successfully!");
    }

    void updateServiceBtn_Click(object sender, EventArgs e)
    {
        if (servicesGrid.CurrentRow == null) return;

        int id = (int)servicesGrid.CurrentRow.Cells["Id"].Value;
        var service = _db.Services.Find(id);
        if (service == null) return;

        bool changed = false;

        if (!string.IsNullOrWhiteSpace(serviceName.Text) &&
            serviceName.Text != service.Name)
        {
            service.Name = serviceName.Text;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(servicePrice.Text) &&
            decimal.TryParse(servicePrice.Text, out var price) &&
            price != service.Price)
        {
            service.Price = price;
            changed = true;
        }

        if (changed)
        {
            _db.SaveChanges();
            LoadServices();
            LoadCarServices();
            MessageBox.Show("Service updated successfully.");
        }
        else
        {
            MessageBox.Show("Nothing to change.");
        }
    }

    void deleteServiceBtn_Click(object sender, EventArgs e)
    {
        if (servicesGrid.CurrentRow == null)
            return;

        int id = (int)servicesGrid.CurrentRow.Cells["Id"].Value;

        var service = _db.Services
            .Include(s => s.CarServices)
            .FirstOrDefault(s => s.Id == id);

        if (service == null)
            return;

        if (service.CarServices.Any())
        {
            MessageBox.Show("Cannot delete this service. Remove related car‑service records first.");
            return;
        }

        _db.Services.Remove(service);
        _db.SaveChanges();

        LoadServices();
        LoadCarServices();
        MessageBox.Show("Service deleted successfully.");
    }

    void addCarServiceBtn_Click(object sender, EventArgs e)
    {
        if (carSelect.SelectedValue == null ||
            serviceSelect.SelectedValue == null ||
            string.IsNullOrWhiteSpace(mileageInput.Text))
        {
            MessageBox.Show("Please fill in all fields (Car, Service, Mileage, Date).");
            return;
        }

        if (!int.TryParse(mileageInput.Text, out int mileage) || mileage <= 0)
        {
            MessageBox.Show("Please enter a valid positive mileage.");
            return;
        }

        _db.CarServices.Add(new CarService
        {
            CarId = (int)carSelect.SelectedValue,
            ServiceId = (int)serviceSelect.SelectedValue,
            Mileage = mileage,
            DateOfService = serviceDate.Value
        });
        _db.SaveChanges();
        LoadCarServices();
        MessageBox.Show("Car service record added successfully!");
    }

    void updateCarServiceBtn_Click(object sender, EventArgs e)
    {
        if (carServiceGrid.CurrentRow == null)
        {
            MessageBox.Show("No record selected.");
            return;
        }

        int carId = (int)carServiceGrid.CurrentRow.Cells["CarId"].Value;
        int serviceId = (int)carServiceGrid.CurrentRow.Cells["ServiceId"].Value;
        DateTime date = (DateTime)carServiceGrid.CurrentRow.Cells["DateOfService"].Value;

        var entity = _db.CarServices.Find(carId, serviceId, date);
        if (entity == null)
        {
            MessageBox.Show("Record not found in database.");
            return;
        }

        bool changed = false;

        // Date change
        if (serviceDate.Value != entity.DateOfService)
        {
            entity.DateOfService = serviceDate.Value;
            changed = true;
        }

        // Mileage change
        if (!string.IsNullOrWhiteSpace(mileageInput.Text)
            && int.TryParse(mileageInput.Text, out int newMileage)
            && newMileage != entity.Mileage)
        {
            entity.Mileage = newMileage;
            changed = true;
        }

        // Optional change: reassign to another car or service
        if ((int)carSelect.SelectedValue != entity.CarId)
        {
            entity.CarId = (int)carSelect.SelectedValue;
            changed = true;
        }

        if ((int)serviceSelect.SelectedValue != entity.ServiceId)
        {
            entity.ServiceId = (int)serviceSelect.SelectedValue;
            changed = true;
        }

        if (changed)
        {
            _db.SaveChanges();
            LoadCarServices();
            MessageBox.Show("Car service record updated successfully.");
        }
        else
        {
            MessageBox.Show("Nothing to change.");
        }
    }

    void deleteCarServiceBtn_Click(object sender, EventArgs e)
    {
        if (carServiceGrid.CurrentRow == null)
            return;

        int carId = (int)carServiceGrid.CurrentRow.Cells["CarId"].Value;
        int serviceId = (int)carServiceGrid.CurrentRow.Cells["ServiceId"].Value;
        DateTime date = (DateTime)carServiceGrid.CurrentRow.Cells["DateOfService"].Value;

        var entry = _db.CarServices.Find(carId, serviceId, date);
        if (entry == null)
            return;

        _db.CarServices.Remove(entry);
        _db.SaveChanges();

        LoadCarServices();
        MessageBox.Show("Car service record deleted.");
    }
}