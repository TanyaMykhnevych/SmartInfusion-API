﻿using Common.Entities;
using System.Collections.Generic;

namespace BusinessLayer.Services.Abstractions
{
    public interface IDiseaseHistoryService
    {
        DiseaseHistory GetDiseaseHistoryById(int id);

        DiseaseHistory GetDiseaseHistoryByPatientId(int id);

        IList<DiseaseHistory> GetAll();

        DiseaseHistory AddDiseaseHistory(DiseaseHistory history);

        IList<DiseaseHistory> GetDiseaseHistoriesByUsername(string userName);

        void Update(DiseaseHistory history);
    }
}
