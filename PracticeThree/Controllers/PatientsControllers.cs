using Microsoft.AspNetCore.Mvc;
using upbThree.CoreLogic.Models;
using upbThree.CoreLogic.Managers;

namespace upbThree.PracticeThree.Controllers;

[ApiController]
[Route("patients")]
public class PatientsController : ControllerBase
{
   private readonly PatientManager _patientsmanager;

   public PatientsController(PatientManager patientmanager)
   {
      _patientsmanager = patientmanager;
   }

   [HttpGet]
   public List<Patients> Get()
   {
      return _patientsmanager.GetAll();
   }

   [HttpGet]
   [Route("{ci}")]
   public Patients GetByID([FromRoute] int ID)
   {
      return _patientsmanager.GetByID(ID);
   }

   [HttpPut]
   [Route("{ci}")]
   public Patients Put([FromRoute] int ID, [FromBody] Patients patientToUpdate)
   {
      return _patientsmanager.Update(ID, patientToUpdate.Nume, patientToUpdate.LN);
   }

   [HttpPost]
   public Patients Post([FromBody]Patients patientToCreate)
   {
      return _patientsmanager.Create(patientToCreate.Nume, patientToCreate.LN, patientToCreate.YearsOnEarth, patientToCreate.ID);
   }

   [HttpDelete]
   [Route("{ci}")]
   public Patients Delete([FromRoute] int ID)
   {
      return _patientsmanager.Delete(ID);
   }
}
