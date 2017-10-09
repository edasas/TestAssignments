using HarbortouchTest.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarbortouchTest.Models
{
    public static class ViewModelHelper
    {
        public static RentalView RentalToViewModel(Rental rental)
        {
            var toReturn = new RentalView
            {
                Name = rental.Name,
                Id = rental.Id,
                OwnerName = rental.OwnerName,
                State = rental.State,
                Address = rental.Address,
                City = rental.City,
                ZipCode = rental.ZipCode
            };

            foreach (var tenantInRental in rental.Tenants)
            {
                toReturn.Tenants.Add(new TenantRentingData
                {
                    Id = tenantInRental.Id,
                    TenantName = tenantInRental.Name,
                    StartDate = tenantInRental.StartDate,
                    EndDate = tenantInRental.EndDate,
                    Cost = tenantInRental.Cost,
                    IsRenting = true,
                });
            }

            return toReturn;
        }

        public static RentalView RentalToViewModel(Rental rental, ICollection<Tenant> allTenants)
        {
            var toReturn = new RentalView
            {
                Name = rental.Name,
                Id = rental.Id,
                OwnerName = rental.OwnerName,
                State = rental.State,
                Address = rental.Address,
                City = rental.City,
                ZipCode = rental.ZipCode,
            };

            ICollection<TenantRentingData> tenantsWithStatus = new List<TenantRentingData>();

            foreach(var tenant in allTenants)
            {
                tenantsWithStatus.Add(new TenantRentingData
                {
                    Id = tenant.Id,
                    Cost = tenant.Cost,
                    StartDate = tenant.StartDate,
                    EndDate = tenant.EndDate,
                    TenantName = tenant.Name,
                    IsRenting = rental.Tenants.FirstOrDefault(rentingTenant => rentingTenant.Id == tenant.Id) != null,
                });
            }

            toReturn.Tenants = tenantsWithStatus;

            return toReturn;
        }

        public static Rental RentalViewToRental(RentalView rentalView)
        {
            var toReturn = new Rental
            {
                Name = rentalView.Name,
                Address = rentalView.Address,
                City = rentalView.City,
                State = rentalView.State,
                OwnerName = rentalView.OwnerName,
                Tenants = new List<Tenant>(),
                ZipCode = rentalView.ZipCode,
            };
            return toReturn;
        }

        public static ICollection<TenantRentingData> GetAllTenantData(HarbortouchContext context)
        {
            var toReturn = new List<TenantRentingData>();
            foreach(var item in context.Tenants)
            {
                toReturn.Add(new TenantRentingData()
                {
                    Id = item.Id,
                    TenantName = item.Name,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Cost = item.Cost,
                    IsRenting = false,
                });
            }
            return toReturn;
        }
    }
}