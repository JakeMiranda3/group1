using System.Collections.Generic;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

/// <summary>
///     The order test controller class
///     @author Colby
///     @version Fall 2026
/// </summary>
public class OrderTestController
{
    #region Data members

    private readonly TestDal testDal;
    private readonly TestOrderDal testOrderDal;

    #endregion

    #region Constructors

    public OrderTestController()
    {
        this.testDal = new TestDal();
        this.testOrderDal = new TestOrderDal();
    }

    #endregion

    #region Methods

    public List<Test> GetAllTest()
    {
        return this.testDal.GetAllTest();
    }

    public List<Test> GetAllTestOrderedByAppointmentId(int appointmentId)
    {
        return this.testOrderDal.GetAllTestOrderedByAppointmentId(appointmentId);
    }

    #endregion

    public void OrderTest(List<Test> orderedTests, int appointmentId)
    {
        this.testOrderDal.OrderTest(orderedTests, appointmentId);
    }
}