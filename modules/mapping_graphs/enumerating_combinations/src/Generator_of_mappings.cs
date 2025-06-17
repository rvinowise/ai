using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace rvinowise.ai.generating_combinations {

public struct Element_to_targets<Element, Target>(Element element, IEnumerable<Target> targets) {
    public readonly Element element = element;
    public readonly List<Target> targets = [..targets];
}
public readonly struct Element_to_target<Element, Target>(Element element, Target target) {
    public readonly Element element = element;
    public readonly Target target = target;

    public override string ToString() {
        return $"{element}-{target}";
    }
}
public class Generator_of_mappings<Element, Target>
//   all_states      subfigures_of_stencil   vertex-to-vertex_mapping   stencil_vertex   figure_vertex
    :IEnumerable<    IEnumerable<            Element_to_target<         Element,         Target        >>>
    
    where Element: notnull 
{
    
    private readonly List<Element_to_targets<Element,Target>> elements_to_targets;
    public Generator_of_mappings(
        IEnumerable<Element_to_targets<Element, Target>> elements_to_targets
    ) {
        this.elements_to_targets = [..elements_to_targets];
    }
    public Generator_of_mappings(
        IEnumerable<(Element, IEnumerable<Target>)> elements_to_targets
    ) {
        this.elements_to_targets = new List<Element_to_targets<Element,Target>>();
        foreach(var element_to_targets in elements_to_targets) {
            (Element element, IEnumerable<Target> targets) = element_to_targets;
            this.elements_to_targets.Add(new Element_to_targets<Element, Target>(element, targets));
        }
    }

    #region IEnumerable

    public IEnumerator<IEnumerable<Element_to_target<Element, Target>>> GetEnumerator() {
        return new Generator_of_mappings_enumerator<Element, Target>(elements_to_targets);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    #endregion IEnumerable
    
}

class Target_counter<Target> {
    private int current;
    private readonly List<Target> targets;

    public Target_counter(List<Target> targets) {
        contracts.Contract.Requires(targets.Any(), 
            "every mapped element should have at least one possible target");
        this.targets = targets;
    }

    public bool MoveNext() {
        current++;
        return current < targets.Count;
    }
    public void SetToFirst() {
        current=0;
    }

    public Target current_target() {
        return targets[current];
    }

    public override string ToString() {
        var str = new StringBuilder();
        for (int i=0;i<current;i++) {
            str.Append($"{targets[i]},");
        }
        str.Append($"({targets[current]})");
        for (int i=current+1;i<targets.Count;i++) {
            str.Append($",{targets[i]}");
        }
        return str.ToString();
    }
}

public class Generator_of_mappings_enumerator<Element, Target>
    :IEnumerator<List<Element_to_target<Element, Target>>>
    where Element: notnull
{

    private readonly List<Element_to_target<Element, Target>> combination = new();

    private readonly List<Tuple<Element, Target_counter<Target>>> elements_to_targets;
    public Generator_of_mappings_enumerator(
        List<Element_to_targets<Element, Target>> elements_to_targets
    ) {
        this.elements_to_targets = 
            new List<Tuple<Element, Target_counter<Target>>>();
        foreach(var element_to_targets in elements_to_targets) {
            this.elements_to_targets.Add(
                Tuple.Create(
                    element_to_targets.element,
                    new Target_counter<Target>(element_to_targets.targets)
                )
            );
        }
        Reset();
    }

    private void set_combination_to_current_targets() {
        combination.Clear();
        foreach(var element_to_target in elements_to_targets) {
            var element = element_to_target.Item1;

            var current_target =
                element_to_target.Item2.current_target();

            combination.Add(new Element_to_target<Element, Target>(element, current_target));
        }
    }
    
  
    private bool first_combination() {
        bool next_combination_exists = true;
        while (
            !correct_combination()
            &&
            next_combination_exists
        ) {
            next_combination_exists = next_mapping();
        }

        return next_combination_exists;
    }

    private bool correct_combination() { 
        set_combination_to_current_targets();
        var all_targets = new HashSet<Target>(); 
        foreach(var pair in combination) {
            all_targets.Add(pair.target);
        }
        return all_targets.Count() == combination.Count;
    }

    private bool next_mapping() {
        using var all_elements = elements_to_targets.GetEnumerator();
        all_elements.MoveNext();

        while (!all_elements.Current.Item2.MoveNext()) {
            all_elements.Current.Item2.SetToFirst();
            if (!all_elements.MoveNext()) {
                return false;
            }
        }
            
        return true;
    }


    #region IEnumerator
    public bool MoveNext()
    {
        bool exist;
        if (!combination.Any()) {
            exist = first_combination();
        } else {
            exist = next_mapping();
        }

        while (
            !correct_combination()
            &&
            exist
        ) {
            exist = next_mapping();
        }
        return exist;
    }

    public void Reset()
    {
        combination.Clear();
    }

    List<Element_to_target<Element, Target>> 
    IEnumerator<List<Element_to_target<Element, Target>>>.Current {
        get { return combination; }
    }

    object IEnumerator.Current {
        get { return combination; }
    }
    
    public void Dispose() {
        GC.SuppressFinalize(this);
    }
    
    #endregion IEnumerator

    

    
}

}