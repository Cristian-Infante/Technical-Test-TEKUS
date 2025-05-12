using Application.DTOs;
using MediatR;

namespace Application.Queries;

public record GetCountryByIsoQuery(string IsoCode) : IRequest<CountryDto?>;
