/*  INTERFACES
 *  Create new interfaces in this file. 
 */

//IAction is implimented on anything that has an activation (e.g. burning an object).
public interface IAction {
    //Called by objects that activate things
    void Activation();

    //Forces the implimentation of a bool for activation state
    bool IsActivated
    {
        get;
    }
}