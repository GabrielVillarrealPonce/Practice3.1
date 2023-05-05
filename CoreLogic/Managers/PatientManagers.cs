using upbThree.CoreLogic.Models;
using System;

namespace upbThree.CoreLogic.Managers;

public class PatientManager
{
    private string _path;
    
    public PatientManager(string path)
    {
        _path = path;
    }

    public List<Patients> GetAll()
    {
        List<Patients> patients = new List<Patients>();
        using (StreamReader reader = new StreamReader(_path))
        {
            string line;
            string[] datos;
            Patients newPatient;
            while(!reader.EndOfStream)
            {
                line = reader.ReadLine();
                datos = line.Split(',');
                newPatient = new Patients() 
                {
                    Nume = datos[0],
                    LN = datos[1],
                    BG = datos[2],
                    YearsOnEarth = int.Parse(datos[3]),
                    ID = int.Parse(datos[4])
                };
                patients.Add(newPatient);
            }   
        }
        return patients;
    }

    public Patients GetByID(int ID)
    {
        if (ID < 0)
        {
            throw new Exception("invalide ID");
        }

        Patients patientfound = Find(patient => int.Parse(patient[2]) == ID);
        if(patientfound == null)
        {
            throw new Exception("Error, Patient not found");
        }
        return patientfound;
    }

    public Patients Update(int ID, string name, string lastname)
    {
        if (ID < 0)
        {
            throw new Exception("invalide ID");
        }

        Patients patientfound = GetByID(ID);

        if (patientfound == null)
        {
            throw new Exception("Error, Patient not found");
        }

        int indexLine = FindIndex(patient => int.Parse(patient[2]) == ID);

        List<string> lines = File.ReadAllLines(_path).ToList();

        if (lines.Count >= indexLine)
        {
            patientfound.Nume = name;
            patientfound.LN =  lastname;
            lines[indexLine] = patientfound.Nume + ", " + patientfound.LN + ", " + patientfound.ID.ToString() + ", " + patientfound.BG + "," + patientfound.YearsOnEarth.ToString();
        }

        using (StreamWriter writer = new StreamWriter(_path))
        {
            foreach (string linea in lines)
            {
                writer.WriteLine(linea);
            }
        }

        return patientfound;
    }
    public Patients Create(string name, string lastname, int age, int ID)
    {
        if(Find(patient => int.Parse(patient[2]) == ID) != null)
        {
            throw new Exception("Error, non existance ID");
        }

        if(ID < 0)
        {
            throw new Exception("Error, invalide ID");
        }

        Random rnd = new Random();
        string[] BG = new string[] {"O+","O-","AB+","AB-","B+","B-","A-","A+"};
        Patients createdPatient = new Patients()
        {
            Nume = name,
            LN = lastname,
            YearsOnEarth = age,
            ID = ID,
            BG = BG[rnd.Next(0,7)]
        };
        Add(createdPatient);
        return createdPatient;
    }

    public Patients Delete(int ID)
    {
        Patients patientToDelete = GetByID(ID);
        int indexLine = FindIndex(patient => int.Parse(patient[2]) == ID);

        if (patientToDelete == null)
        {
            throw new Exception("Error, Patient not found");
        }
        List<string> lines = File.ReadAllLines(_path).ToList();
        if (lines.Count >= indexLine)
        {
            lines.RemoveAt(indexLine);
        }
        using (StreamWriter writer = new StreamWriter(_path))
        {
            foreach (string linea in lines)
            {
                writer.WriteLine(linea);
            }
        }
        return patientToDelete;
    }

    public Patients Find(Predicate<string[]> predicate)
    {
        Patients patient = null;
        StreamReader reader = new StreamReader(@_path);
        string line;
        string[] datos;
        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            if(line != null)
            {
                datos = line.Split(",");
                if (predicate(datos))
                {
                    patient = new Patients()
                    {
                        Nume = datos[0],
                        LN = datos[1],
                        ID = int.Parse(datos[2]),
                        BG = datos[3],
                        YearsOnEarth = int.Parse(datos[4])
                    };
                    
                    break;
                }
            }
        }
        reader.Close();
        return patient;
    }

    public void Add(Patients patient)
    {
        string paciente = patient.Nume + ", " + patient.LN + ", " + patient.ID.ToString() + ", " + patient.BG + "," + patient.YearsOnEarth.ToString();
        try
        {
            using (FileStream fs = new FileStream(@_path, FileMode.Append))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(paciente);
                sw.Close();
            }
        }
        catch (Exception e)
        {
            throw new Exception("Error: "+ e.Message);
        }
    }

    public int FindIndex(Predicate<string[]> predicate)
    {
        int indexLine = 0;
        using (StreamReader reader = new StreamReader(_path))
        {
            string line;
            string[] datos;
            while(!reader.EndOfStream)
            {
                line = reader.ReadLine();
                datos = line.Split(',');
                if(predicate(datos))
                {
                    break;
                }
                indexLine++;
            }
        }
        return indexLine;
    }    
}