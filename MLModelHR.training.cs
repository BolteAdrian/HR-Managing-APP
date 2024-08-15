﻿﻿// This file was auto-generated by ML.NET Model Builder. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML;

namespace HR
{
    public partial class MLModelHR
    {
        /// <summary>
        /// Retrains model using the pipeline generated as part of the training process. For more information on how to load data, see aka.ms/loaddata.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="trainData"></param>
        /// <returns></returns>
        public static ITransformer RetrainPipeline(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"Name",outputColumnName:@"Name")      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"Function Apply",outputColumnName:@"Function Apply"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"Function Match",outputColumnName:@"Function Match"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"Studies",outputColumnName:@"Studies"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"Experience",outputColumnName:@"Experience"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"Observation",outputColumnName:@"Observation"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"Mode Apply",outputColumnName:@"Mode Apply"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"County",outputColumnName:@"County"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"City",outputColumnName:@"City"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"BirthDate",outputColumnName:@"BirthDate"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"Status",outputColumnName:@"Status"))      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"Name",@"Function Apply",@"Function Match",@"Studies",@"Experience",@"Observation",@"Mode Apply",@"County",@"City",@"BirthDate",@"Status"}))      
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName:@"OfferStatus",inputColumnName:@"OfferStatus"))      
                                    .Append(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"))      
                                    .Append(mlContext.MulticlassClassification.Trainers.OneVersusAll(binaryEstimator: mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(new LbfgsLogisticRegressionBinaryTrainer.Options(){L1Regularization=0.03125F,L2Regularization=14.32019F,LabelColumnName=@"OfferStatus",FeatureColumnName=@"Features"}), labelColumnName:@"OfferStatus"))      
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName:@"PredictedLabel",inputColumnName:@"PredictedLabel"));

            return pipeline;
        }
    }
}
