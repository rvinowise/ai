using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.contracts;


namespace rvinowise.ai.mapping_stencils {


/*
    iterates in the numerical order, with each Order being another iterator
    (with Reset and MoveNext methods) 
    */


public class Generator_of_orders<T>:
    IEnumerable<IEnumerable<T>>
{
    private readonly List<IEnumerable<T>> orders;

    public Generator_of_orders(IEnumerable<IEnumerable<T>> orders) {
        this.orders = new List<IEnumerable<T>>(orders);
    }

    private IEnumerable<T> get_combination_as_indexes(
        IEnumerable<IEnumerator<T>> mapping_enumerators
    ) {
        T[] result_orders = new T[mapping_enumerators.Count()];
        
        int i_figure=0;
        foreach(IEnumerator<T> figure_enumerator in mapping_enumerators) {
            result_orders[i_figure++] = figure_enumerator.Current;
        }

        return result_orders;
    }
    
    #region IEnumerable

    public IEnumerator<IEnumerable<T>> GetEnumerator() {
        if (!orders.Any()) {
            throw new Broken_contract_exception("enumerable orders should be added to the generator before enumerating it");
        }
        
        IList<IEnumerator<T>> order_enumerators = orders.Select(
            order => order.GetEnumerator()
        ).ToList();
        foreach(var enumerator in order_enumerators) {
            enumerator.SetToFirst();
        }
        yield return get_combination_as_indexes(order_enumerators);
        while (set_next_iteration(order_enumerators)) {
            yield return get_combination_as_indexes(order_enumerators);
        }
    }

    

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    #endregion IEnumerable

    private bool set_next_iteration(IEnumerable<IEnumerator> enumerators) {
        foreach (IEnumerator enumerator in enumerators) {
            if (enumerator.MoveNext()) {
                return true;
            }
            enumerator.SetToFirst();
        }
        return false;
    }

    
}





}