﻿using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Fuse8_ByteMinds.SummerSchool.InternalApi.Models;
using InternalApi.Interfaces;
using InternalApi.Models;

using Microsoft.AspNetCore.Mvc;

namespace InternalApi.Controllers
{
    /// <summary>
    /// Методы для обращения к внешнему API https://api.currencyapi.com
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICachedCurrencyAPI _cachedCurrencyAPI;

        public CurrencyController(ICachedCurrencyAPI cachedCurrencyAPI) => _cachedCurrencyAPI = cachedCurrencyAPI;

        /// <summary>
        /// Получить курс валюты по коду
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="currencyCode">Код валюты</param>
        /// <response code="200">
        /// Возвращает если удалось получить курс валюты
        /// </response>
        /// <response code="404">
        /// Возвращает при неизвестном коде валюты
        /// </response>
        /// /// <response code="429">
        /// Возвращает если удалось не удалось получить доступ к API из-за исчерпания лимита
        /// </response>
        /// <response code="500">
        /// Возвращает при иной ошибке
        /// </response>
        /// <returns>Ответ на запрос курса валюты на последнюю дату</returns>
        [HttpGet("{currencyCode}")]
        public async Task<CurrencyDTO> GetLatestAsync(CurrencyCode currencyCode, CancellationToken cancellationToken)
        {
            //НУЖНО РЕАЛИЗОВАТЬ ХЕЛСЧЕК


            return await _cachedCurrencyAPI.GetCurrentCurrencyAsync(currencyCode, cancellationToken);
        }

        /// <summary>
        /// Получить курс валюты по коду с указанием даты актуальности
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата, на которую актуален курс</param>
        /// <response code="200">
        /// Возвращает если удалось получить курс валюты
        /// </response>
        /// <response code="404">
        /// Возвращает при неизвестном коде валюты
        /// </response>
        /// /// <response code="429">
        /// Возвращает если удалось не удалось получить доступ к API из-за исчерпания лимита
        /// </response>
        /// <response code="500">
        /// Возвращает при иной ошибке
        /// </response>
        /// <returns>Ответ на запрос курса валюты с указанием даты актуальности курса</returns>
        [HttpGet("{currencyCode}/{date}")]
        public async Task<CurrencyDTO> GetHistoricalAsync(CurrencyCode currencyCode, DateOnly date, CancellationToken cancellationToken)
        {
            return await _cachedCurrencyAPI.GetCurrencyOnDateAsync(currencyCode, date, cancellationToken);
        }

        /// <summary>
        /// Запрос текущих настроек приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает если удалось получить настройки
        /// </response>
        /// <response code="404">
        /// Возвращает при неизвестном коде валюты
        /// </response>
        /// <response code="429">
        /// Возвращает если удалось не удалось получить доступ к API из-за исчерпания лимита
        /// </response>
        /// <response code="500">
        /// Возвращает при иной ошибке
        /// <returns>Ответ на запрос текущих настроек приложения</returns>
        [HttpGet("Settings")]
        public async Task<GetSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken)
        {
            return await _cachedCurrencyAPI.GetSettingsAsync(cancellationToken);
        }
    }
}