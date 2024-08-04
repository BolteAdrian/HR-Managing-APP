﻿using HR.Models;
using HR.Repository;
using HR.Utils;
using HRML.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR.Controllers
{
    public class PredictionsController : Controller
    {
        private readonly modelContext _context;

        public PredictionsController(modelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a paginated and sorted list of person CVs with optional filtering.
        /// </summary>
        /// <param name="filter">Filter string to search for in person CVs.</param>
        /// <param name="page">Current page number for pagination.</param>
        /// <param name="sortExpression">Expression to sort the person CVs.</param>
        /// <returns>Returns a view with a list of person CVs.</returns>
        public async Task<IActionResult> Index(string filter, int page = 1, string sortExpression = "Id")
        {
            try
            {
                List<Candidates> personCvList = await _context.Candidates.AsNoTracking().OrderBy(p => p.Id).ToListAsync();
                var query = _context.Candidates.AsNoTracking().OrderBy(p => p.Id).AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query = query.Where(p => p.Name.Contains(filter) || p.City.Contains(filter) || p.County.Contains(filter));
                }

                var model = await PagingList.CreateAsync(query, 10, page, sortExpression, "Name");
                model.RouteValue = new RouteValueDictionary { { "filter", filter } };

                List<Functions> functions = await _context.Functions.AsNoTracking().OrderBy(p => p.Id).ToListAsync();
                List<ModelInput> modelInputs = new List<ModelInput>();

                foreach (var personCv in personCvList)
                {
                    if (personCv.Status != 2)
                    {
                        ModelInput modelInput = new ModelInput
                        {
                            Id = personCv.Id,
                            BirthDate = personCv.BirthDate?.ToShortDateString() ?? Constants.PREDICTION.NoDataFound
                        };

                        long functionId = (long)personCv.FunctionApply;
                        var function = functions.FirstOrDefault(f => f.Id == functionId);
                        if (function != null)
                        {
                            modelInput.FunctionName = function.Name;
                            var department = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == function.DepartmentId);
                            modelInput.DepartmentName = department?.Name ?? Constants.PREDICTION.NoDataFound;
                        }

                        modelInput.ModeApply = (float)personCv.ModeApply;
                        modelInput.Studies = personCv.Studies;
                        modelInput.Experience = personCv.Experience;
                        modelInput.Observation = personCv.Observation;

                        modelInputs.Add(modelInput);
                    }
                }

                if (!modelInputs.Any())
                {
                    return View(model);
                }

                var predictions = new Dictionary<int, float>();
                foreach (var modelInput in modelInputs)
                {
                    // Load model and predict output of sample data
                    ModelOutput result = ConsumeModel.Predict(modelInput);
                    predictions.Add((int)modelInput.Id, result.Score[0] * 100);
                }

                List<Functions> functionList = await _context.Functions.AsNoTracking().ToListAsync();
                ViewData["FunctionApply"] = functionList;
                ViewData["FunctionMatch"] = functionList;
                ViewBag.Predictions = predictions;

                return View(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }
    }
}
