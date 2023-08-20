﻿using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public interface IAppDbContext
    {
        /// <summary>
        /// Настройки приложения в базе данных
        /// </summary>
        DbSet<Settings> Settings { get; }

        /// <summary>
        /// Избранные валюты
        /// </summary>
        DbSet<FavoriteCurrency> FavoriteCurrencies { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
