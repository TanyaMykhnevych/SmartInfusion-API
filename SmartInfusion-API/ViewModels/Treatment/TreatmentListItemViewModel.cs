﻿using Common.Entities;

namespace SmartInfusion.API.ViewModels
{
    public class TreatmentListItemViewModel
    {
        public int TreatmentId { get; set; }

        public int MedicineId { get; set; }

        public string Diagnosis { get; set; }

        public double MedicineWeight { get; set; }

        public double SolutionVolume { get; set; }

        public double Dosage { get; set; }

        public int DiseaseHistoryId { get; set; }

        public TreatmentListItemViewModel(Treatment treatment)
        {
            TreatmentId = treatment.TreatmentId;
            MedicineId = treatment.MedicineId;
            Diagnosis = treatment.Diagnosis;
            MedicineWeight = treatment.MedicineWeight;
            SolutionVolume = treatment.SolutionVolume;
            Dosage = treatment.Dosage;
            DiseaseHistoryId = treatment.DiseaseHistoryId;
        }
    }
}
