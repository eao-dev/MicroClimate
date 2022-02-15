using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MicroClimateControllSystem.Models;

namespace MicroClimateControllSystem.Controllers
{
    public class MainController : Controller
    {
        private ClimateContext db = new ClimateContext();

        // Добавление данных с устройства
        public bool Get(SensorData sensorData, Sensor sensorInput) {
            try
            {
                Sensor sensor = db.Sensor.Find(sensorData.SensorID);
                if (sensor != null && (sensor.Password == sensorInput.Password))
                {
                    sensorData.Sensor = sensor;
                    sensorData.DateTime = DateTime.Now;
                    db.SensorsData.Add(sensorData);
                    db.SaveChanges();
                }
                else
                    return false;

            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }
            return true;
        }

        // Просмотр данных с датчика
        public ActionResult ViewSensor(Sensor inputSensor)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    // Конкретный датчик
                    // todo
                    Sensor sensor = db.Sensor.Include(path => path.SensorData).Where(p => p.SensorID == inputSensor.SensorID).ToList()[0];
                    return View(sensor);
                }
                catch {
                    // Список всех датчиков
                    return RedirectToAction("Index", "Main");
                }
            }
            else
                return new HttpUnauthorizedResult();
        }

        // Список всех датчиков
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Список всех датчиков
                List<Sensor> sensors = db.Sensor.ToList();
                return View(sensors);
            }
            else
                return new HttpUnauthorizedResult();
        }

        // Вывод теущих значений
        public ActionResult CurrentValue() {
            if (User.Identity.IsAuthenticated)
                return PartialView();
            else
                return new HttpUnauthorizedResult();
        }

        // Добавить датчик
        public ActionResult AddSensor()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
                return new HttpUnauthorizedResult();
        }

        public bool sensorAdd(ref Sensor sensor)
        {
            try
            {
                sensor.Date = DateTime.Today;
                db.Sensor.Add(sensor);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Добавить датчик
        [HttpPost]
        public ActionResult AddSensor(Sensor sensor)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (sensorAdd(ref sensor))
                {
                    ViewBag.Message = "Датчик " + sensor.SensorID + " добавлен!";
                    return View("~/Views/Shared/Message.cshtml");
                }
                else
                {
                    ViewBag.Error = "Ошибка добавления датчика!";
                    return View("~/Views/Shared/Message.cshtml");
                }
            }
            else
                return new HttpUnauthorizedResult();
        }

        // Графики
        public ActionResult Graphs()
        {
            if (User.Identity.IsAuthenticated)
                return PartialView();
            else
                return new HttpUnauthorizedResult();
        }

        // Стаитистика
        public ActionResult Stats()
        {
            if (User.Identity.IsAuthenticated)
                return PartialView();
            else
                return new HttpUnauthorizedResult();
        }

        // Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return new HttpUnauthorizedResult();
        }

        public bool delAccountByName(string name)
        {
            try 
            { 
                var auth = db.Auth.First(p => p.Login == name);
                if (auth != null)
                {
                    db.Auth.Remove(auth);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        // Удалить аккаунт
        [HttpGet]
        public ActionResult DelAccount()
        {
            if (!delAccountByName(User.Identity.Name))
            {
                ViewBag.Error = "Не удалось удалить учётную запись!";
                return View("~/Views/Shared/Message.cshtml");
            }
            return new HttpUnauthorizedResult();
        }

        // Обновление данных об устройстве
        public ActionResult Edit(Sensor inputSensor)
        {
            if (User.Identity.IsAuthenticated)
            {
                Sensor sensor = db.Sensor.Find(inputSensor.SensorID);
                return View(sensor);
            }
            else
                return new HttpUnauthorizedResult();
        }

        // Измненение конфигурации сенсороа
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditValue(Sensor inputSensor)
        {
            if (User.Identity.IsAuthenticated)
            {
                Sensor sensor = db.Sensor.Find(inputSensor.SensorID);

                if (ModelState.IsValid)
                {
                    sensor.Location = inputSensor.Location;
                    sensor.NormalTemp = inputSensor.NormalTemp;
                    sensor.NormalHumidity = inputSensor.NormalHumidity;
                    db.Entry(sensor).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.Error = "Ошибка обновления";
                        return View("~/Views/Shared/Message.cshtml");
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Некорректные данные!";
                    return View("~/Views/Shared/Message.cshtml");
                }
            }
            else
                return new HttpUnauthorizedResult();
        }

        // Удаление 
        public ActionResult DeleteSensor(Sensor inputSensor)
        {
            if (User.Identity.IsAuthenticated)
            {
                Sensor sensor = db.Sensor.Find(inputSensor.SensorID);
                if (sensor != null)
                {
                    db.Sensor.Remove(sensor);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Main");
                }
                else {
                    ViewBag.Error = "Ошибка! Датчик не найден!";
                    return View("~/Views/Shared/Message.cshtml");
                }
            }
            else {
                ViewBag.Error = "Ошибка удаления датчика!";
                return View("~/Views/Shared/Message.cshtml");
            }
        }
    }
}
