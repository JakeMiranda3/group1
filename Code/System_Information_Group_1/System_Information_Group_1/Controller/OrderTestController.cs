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

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderTestController"/> class.
    /// </summary>
    public OrderTestController()
    {
        this.testDal = new TestDal();
        this.testOrderDal = new TestOrderDal();
        
    }

    #endregion

    #region Methods    
    /// <summary>
    /// Gets all test.
    /// </summary>
    /// <returns>All the tests ever</returns>
    public List<Test> GetAllTest()
    {
        return this.testDal.GetAllTest();
    }

    /// <summary>
    /// Gets the test already ordered by appointment identifier.
    /// </summary>
    /// <param name="appointmentId">The appointment identifier.</param>
    /// <returns>The tests ordered for an appointment</returns>
    public List<Test> GetTestAlreadyOrderedByAppointmentId(int appointmentId)
    {
        return this.testOrderDal.GetAllTestOrderedByAppointmentId(appointmentId);
    }

    #endregion    
    /// <summary>
    /// Orders the test.
    /// </summary>
    /// <param name="orderedTests">The ordered tests.</param>
    /// <param name="appointmentId">The appointment identifier.</param>
    public void OrderTest(List<Test> orderedTests, int appointmentId)
    {
        this.testOrderDal.OrderTest(orderedTests, appointmentId);
    }
}