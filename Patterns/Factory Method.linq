<Query Kind="Program" />

// Factory Method pattern
// https://www.dofactory.com/net/factory-method-design-pattern

void Main()
{
	List<Document> documents = new List<Document>();
	documents.Add(new Resume());
	documents.Add(new Report());
	
	foreach (Document document in documents)
	{
		document.Show();
	}
}

abstract class Page { }
class SkillsPage : Page { }
class EducationPage : Page { }
class ExperiencePage : Page { }
class IntroductionPage : Page { }
class ResultsPage : Page { }
class ConclusionPage : Page { }
class SummaryPage : Page { }
class BibliographyPage : Page { }

abstract class Document
{
	private List<Page> _pages = new List<Page>();
	public Document() { this.CreatePages();}
	public List<Page> Pages { get { return _pages; } }
	public abstract void CreatePages();
	public void Show()
	{
		Console.WriteLine($"{this.GetType().Name}:");
		_pages.ForEach(x => Console.WriteLine($"  {x.GetType().Name}") );
	}
}

class Resume : Document
{
	public override void CreatePages()
	{
		Pages.Add(new SkillsPage());
		Pages.Add(new EducationPage());
		Pages.Add(new ExperiencePage());
	}
}

class Report : Document
{
	public override void CreatePages()
	{
		Pages.Add(new IntroductionPage());
		Pages.Add(new ResultsPage());
		Pages.Add(new ConclusionPage());
		Pages.Add(new SummaryPage());
		Pages.Add(new BibliographyPage());
	}
}