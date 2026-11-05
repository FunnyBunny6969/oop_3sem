using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 22: проектирование и UML ===");
        Console.WriteLine("--- Основное задание, вариант 3: система \"Больница\" ---");
        MainTask.Run();

        Section("Дополнительное задание 1: расширенная библиотека");
        Dop1.Run();

        Section("Дополнительное задание 2: интернет-магазин со скидками");
        Dop2.Run();

        Section("Дополнительное задание 3: бронирование номеров в гостинице");
        Dop3.Run();
    }

    static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 80));
        Console.WriteLine("=== " + title + " ===");
        Console.WriteLine(new string('=', 80));
    }
}

public static class MainTask
{
    public class MedicalRecord
    {
        public DateTime Date { get; init; }
        public string Diagnosis { get; init; } = string.Empty;
        public string DoctorName { get; init; } = string.Empty;
        public string Prescription { get; init; } = string.Empty;
    }

    public class Patient
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public List<MedicalRecord> Records { get; } = new List<MedicalRecord>();

        public void AddRecord(MedicalRecord record) => Records.Add(record);
    }

    public class Department
    {
        public string Name { get; init; } = string.Empty;
        public List<Doctor> Doctors { get; } = new List<Doctor>();

        public void AddDoctor(Doctor doctor)
        {
            doctor.Department = this;
            Doctors.Add(doctor);
        }
    }

    public class Doctor
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Specialization { get; init; } = string.Empty;
        public Department? Department { get; set; }
        public List<Appointment> Appointments { get; } = new List<Appointment>();
    }

    public class Appointment
    {
        public int Id { get; init; }
        public Patient Patient { get; init; } = null!;
        public Doctor Doctor { get; init; } = null!;
        public DateTime Date { get; init; }
        public string Status { get; private set; } = "Запланирован";

        public void Complete(string diagnosis, string prescription)
        {
            Status = "Завершён";
            Patient.AddRecord(new MedicalRecord
            {
                Date = DateTime.Now,
                Diagnosis = diagnosis,
                DoctorName = Doctor.Name,
                Prescription = prescription
            });
        }
    }

    public class Hospital
    {
        private int _nextAppointmentId = 1;
        private readonly List<Department> _departments = new List<Department>();
        private readonly List<Patient> _patients = new List<Patient>();
        private readonly List<Appointment> _appointments = new List<Appointment>();

        public void AddDepartment(Department department) => _departments.Add(department);
        public void AddPatient(Patient patient) => _patients.Add(patient);

        public Doctor FindDoctor(int id) =>
            _departments.SelectMany(d => d.Doctors).FirstOrDefault(d => d.Id == id)
            ?? throw new ArgumentException($"Врач с id {id} не найден.");

        public Patient FindPatient(int id) =>
            _patients.FirstOrDefault(p => p.Id == id)
            ?? throw new ArgumentException($"Пациент с id {id} не найден.");

        public Appointment BookAppointment(int patientId, int doctorId, DateTime date)
        {
            Console.WriteLine($"  [Регистратура] Пациент {patientId} запрашивает приём у врача {doctorId} на {date:dd.MM.yyyy HH:mm}");
            Patient patient = FindPatient(patientId);
            Console.WriteLine($"  [Регистратура] Пациент найден: {patient.Name}");

            Doctor doctor = FindDoctor(doctorId);
            Console.WriteLine($"  [Регистратура] Врач найден: {doctor.Name} ({doctor.Specialization})");

            if (doctor.Appointments.Any(a => a.Date == date && a.Status == "Запланирован"))
            {
                Console.WriteLine("  [Регистратура] Время у врача уже занято.");
                throw new InvalidOperationException("Выбранное время уже занято.");
            }

            Appointment appointment = new Appointment
            {
                Id = _nextAppointmentId++,
                Patient = patient,
                Doctor = doctor,
                Date = date
            };

            doctor.Appointments.Add(appointment);
            _appointments.Add(appointment);
            Console.WriteLine($"  [Регистратура] Запись создана, номер {appointment.Id}, статус: {appointment.Status}");
            return appointment;
        }

        public void ShowPatientHistory(int patientId)
        {
            Patient patient = FindPatient(patientId);
            Console.WriteLine($"\nИстория болезни пациента {patient.Name}:");
            if (patient.Records.Count == 0)
            {
                Console.WriteLine("  Записей нет.");
                return;
            }

            foreach (MedicalRecord record in patient.Records)
            {
                Console.WriteLine($"  {record.Date:dd.MM.yyyy}: {record.Diagnosis}, врач {record.DoctorName}, назначение: {record.Prescription}");
            }
        }

        public void ShowAppointments()
        {
            Console.WriteLine("\nВсе приёмы:");
            foreach (Appointment appointment in _appointments)
            {
                Console.WriteLine($"  №{appointment.Id}: {appointment.Patient.Name} -> {appointment.Doctor.Name} ({appointment.Doctor.Specialization}), {appointment.Date:dd.MM.yyyy HH:mm}, {appointment.Status}");
            }
        }
    }

    public static void Run()
    {
        Hospital hospital = new Hospital();

        Department therapy = new Department { Name = "Терапия" };
        Department cardiology = new Department { Name = "Кардиология" };

        Doctor therapist = new Doctor { Id = 1, Name = "Иванов Иван Иванович", Specialization = "Терапевт" };
        Doctor cardiologist = new Doctor { Id = 2, Name = "Петрова Анна Сергеевна", Specialization = "Кардиолог" };
        therapy.AddDoctor(therapist);
        cardiology.AddDoctor(cardiologist);

        hospital.AddDepartment(therapy);
        hospital.AddDepartment(cardiology);

        Patient patient = new Patient { Id = 1, Name = "Сидоров Петр Алексеевич" };
        hospital.AddPatient(patient);

        Console.WriteLine("Запись на приём:");
        Appointment first = hospital.BookAppointment(1, 1, DateTime.Today.AddDays(1).AddHours(10));
        hospital.BookAppointment(1, 2, DateTime.Today.AddDays(2).AddHours(12));

        Console.WriteLine("\nЗавершение приёма:");
        first.Complete("ОРВИ", "Постельный режим, обильное питьё");
        Console.WriteLine($"Приём №{first.Id} завершён, пациенту добавлена запись.");

        hospital.ShowPatientHistory(1);
        hospital.ShowAppointments();
    }
}
