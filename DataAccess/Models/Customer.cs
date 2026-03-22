using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Customer
{

    public int CustomerId { get; set; }

    public string? CustomerFullName { get; set; }

    public string? Telephone { get; set; }

    public string EmailAddress { get; set; } = null!;

    public DateTime? CustomerBirthday { get; set; }

    public byte? CustomerStatus { get; set; }

    public string? Password { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<BookingReservation> BookingReservations { get; set; } = new List<BookingReservation>();

    public virtual Role? Role { get; set; }

    public Customer(string? customerFullName, string? telephone, string emailAddress, DateTime? customerBirthday, byte? customerStatus, string? password, int? roleId)
    {
        CustomerFullName = customerFullName;
        Telephone = telephone;
        EmailAddress = emailAddress;
        CustomerBirthday = customerBirthday;
        CustomerStatus = customerStatus;
        Password = password;
        RoleId = roleId;
    }

    public Customer()
    {
    }
}

