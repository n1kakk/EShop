﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
	public void Configure(EntityTypeBuilder<Customer> builder)
	{
		builder.HasKey(c => c.Id); // первичный ключ
		builder.Property(c => c.Id).HasConversion( // определяет, как свойство Id должно быть преобразовано при чтении из базы данных и записи в нее
				customerId => customerId.Value,
				dbId => CustomerId.Of(dbId));

		builder.Property(c => c.Name).HasMaxLength(50).IsRequired();

		builder.Property(c => c.Email).HasMaxLength(100); 
		builder.HasIndex(c => c.Email).IsUnique();
	}
}
