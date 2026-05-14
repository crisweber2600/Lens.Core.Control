Bottom Up vs Top Down
Pasted text(3).txt
Document
Reimagining LENS Framework(1).pdf
PDF
take this info and also design a bottom up approach as well. Most things start small and then grow 
and gain functionality instead of how northstar is and started top down. We need to be able to 
start small and grow and change as stakeholders change their mind it needs to support both 
mental models. 
Here’s the equivalent of “step 18” but redesigned for the new bottom-up growth-thread mental model
using your 3D printing example instead of a top-down NorthStar-style system. This is the missing half of
LENS: systems that emerge instead of being pre-designed. 
Reimagining LENS Framework
18B. Example: digesting a bottom-up evolving project
Initial user input
I want to download images from 3D printing model websites.
That is all the user knows initially.
There is:
no PRD
no architecture
no system thesis
no defined product
no roadmap
no outcome matrix
no service map
The user only knows:
“I need this one useful thing.”
The new LENS must support this mode equally well.
Core difference from top-down discovery
Top-down model
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
1/48
Large known vision
→ discover structure
→ decompose system
→ slice implementation
Bottom-up growth model
Useful local utility
→ creates artifacts
→ reveals new opportunities
→ reveals new risks
→ reveals reusable capabilities
→ gradually becomes coherent system
This is critical because most real products start this way.
Step 1 — Create tiny seed
Bash
lens seed start \--statement "Download images from a 3D model website"
LENS response
YAML
seed:
id: seed.download_model_images
status: raw
confidence: high
statement: >
Download images from a 3D model website.
known_context:
actor: user
local_need: >
User wants local access to model listing images.
unknowns:- supported websites- image storage format- future purpose- metadata needs- downstream usage- safety requirements- whether this is personal tooling or larger platform
system_status:
current_system_thesis: unknown
recommended_next_step:
lens explore seed seed.download_model_images
Notice:
no fake assumptions
no premature “platform”
no invented architecture
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
2/48
Step 2 — Guided exploration
Bash
lens explore seed seed.download_model_images
LENS begins interactive discovery.
LENS asks focused questions
What are the images used for?
Do you need only thumbnails or full resolution?
Should the system remember where the images came from?
Do duplicate models matter?
Will humans review the images later?
Is this personal organization or a larger workflow?
Will the system eventually analyze the models?
User responds
At first I just want local copies.
Later maybe I want to generate descriptions from them.
Eventually I might want to detect violent or unsafe models.
THIS is the key moment.
The system is beginning to emerge.
Step 3 — Create growth thread
LENS recognizes:
YAML
these are not unrelated ideas
they form a coherent evolution chain
growth_thread:
id: growth_thread.3d_model_intake
status: emerging
maturity: exploratory
origin_seed:- seed.download_model_images
emerging_direction:- image collection- metadata enrichment- model understanding- safety analysis
current_project_thesis:
status: hypothesis
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
3/48
statement: >
A system for collecting, enriching, understanding, and classifying
3D model listing content.
This is NOT a finalized project.
It is a living hypothesis.
Step 4 — Focus first slice only
LENS refuses to plan the future system yet.
Instead:
Bash
lens focus seed seed.download_model_images
lens grow slice
Generated first slice
YAML
slice:
id: slice.download_model_images
status: selected
maturity: implementation_candidate
goal: >
Download images from a supported 3D model listing page.
why_first: >
This creates the foundational artifact needed for later enrichment,
organization, and safety analysis.
starts_with:- model listing URL exists
ends_with:- images downloaded locally- source URLs recorded- images grouped by listing
included_scope:- parse listing page- identify image URLs- download images- record metadata
explicit_out_of_scope:- AI analysis- safety detection- duplicate detection- geometry understanding- marketplace moderation- recommendations- model categorization
acceptance_evidence:- images successfully download- metadata stored- duplicate download prevention works
This is SMALL.
That is intentional.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
4/48
Step 5 — Derive first capabilities
Bash
lens map capabilities \--slice slice.download_model_images
Output
YAML
capabilities:- capability.model_listing_parser- capability.image_download- capability.image_storage- capability.image_source_tracking
candidate_domains:- domain.model_intake
candidate_services:
status: deferred
open_questions:- Should metadata persist in database?- Are images versioned?- Should unsupported sites fail gracefully?
Notice:
no giant architecture
no over-modeling
services deferred until justified
This directly follows the “derive capabilities later” principle from the redesign. 
Reimagining LENS Framework
Step 6 — Run BMAD only for current slice
Bash
lens prepare bmad \--slice slice.download_model_images
BMAD now runs:
Bash
bmad-product-brief
bmad-create-prd
bmad-create-architecture
bmad-create-epics-and-stories
But ONLY for this tiny slice.
Not the future platform.
Step 7 — Implementation reveals new reality
After building:
The downloaded images are surprisingly useful.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
5/48
The user now wants:
auto descriptions
duplicate detection
tagging
moderation
This was NOT known earlier.
Step 8 — Follow-on seed discovery
Bash
lens discover followons \--slice slice.download_model_images
LENS analyzes produced artifacts
YAML
produced_artifacts:- artifact.model_image_set- artifact.source_metadata- artifact.download_history
Newly discovered seeds
YAML
follow_on_seeds:- id: seed.generate_model_description
reason: >
Downloaded image sets can now be analyzed to summarize model contents.- id: seed.detect_duplicate_models
reason: >
Multiple listings may reuse identical image sets.- id: seed.detect_unsafe_models
reason: >
Images may contain violent or unsafe printable objects.- id: seed.extract_listing_metadata
reason: >
Images alone may not provide enough future context.
This is the most important change in the entire framework.
The framework now supports:
implemented work
→ creates new artifacts
→ artifacts reveal new possibilities
→ new possibilities become seeds
→ seeds become slices
→ slices become capabilities
→ capabilities become coherent system
Step 9 — Second slice emerges naturally
User focuses:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
6/48
Bash
lens focus seed seed.generate_model_description
LENS generates next slice
YAML
slice:
id: slice.generate_model_description
goal: >
Generate a textual summary from downloaded model images.
depends_on:- slice.download_model_images
consumes:- artifact.model_image_set
produces:- artifact.model_description
included_scope:- analyze images- summarize visible object- identify likely category- capture uncertainty
explicit_out_of_scope:- moderation decisions- geometry analysis- marketplace publishing- safety enforcement
acceptance_evidence:- descriptions reference visible image evidence- uncertain cases are labeled uncertain
Now the system is becoming:
intake
→ enrichment
Step 10 — Safety slice emerges later
Only AFTER:
images exist
descriptions exist
metadata exists
does this become viable:
Bash
lens focus seed seed.detect_unsafe_models
Generated slice
YAML
slice:
id: slice.detect_unsafe_models
goal: >
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
7/48
Detect potentially violent or unsafe printable models.
depends_on:- slice.download_model_images- slice.generate_model_description
consumes:- artifact.model_image_set- artifact.model_description
produces:- artifact.safety_classification
required_capabilities:- capability.image_classification- capability.safety_taxonomy- capability.review_queue
risks:- false positives- false negatives- ambiguous models- policy disagreement
required_decisions:- what counts as unsafe?- advisory or enforcement?- human review required?
Notice how the architecture emerges
At the beginning:
download images
Later:
3D model intake pipeline
Later:
enrichment and classification system
Later:
moderation workflow
Later maybe:
marketplace safety platform
The user did NOT know this at the beginning.
The framework allowed:
discovery
evolution
promotion
reframing
without forcing fake certainty.
The evolving landscape
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
8/48
Eventually the Living Landscape becomes:
YAML
system:
id: system.3d_model_intelligence_pipeline
domains:- domain.model_intake- domain.model_enrichment- domain.model_safety
capabilities:- capability.image_download- capability.image_normalization- capability.model_description_generation- capability.safety_classification- capability.review_workflow
journeys:- journey.listing_to_local_images- journey.images_to_description- journey.description_to_safety_review
active_slices:- slice.download_model_images- slice.generate_model_description- slice.detect_unsafe_models
This emerged organically.
Not through upfront decomposition.
The key BMAD interaction
LENS does NOT replace BMAD.
Instead:
LENS:
discovers evolving system
BMAD:
plans current focused slice
At every stage:
LENS:
what is emerging?
BMAD:
how do we build this slice correctly?
The crucial new behavior
The old mental model was:
discover everything first
→ then implement
The new model is:
discover enough for one valuable slice
→ implement
→ learn
→ update landscape
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
9/48
→ discover next slice
→ repeat
That is the architecture required for:
startups
evolving products
research systems
AI-native workflows
exploratory platforms
internal tooling
enterprise systems that grow over years
And it directly solves the problem identified in the redesign docs:
Large systems are not specified once.
They are progressively discovered, built, validated, and reorganized.
Reimagining LENS Framework
Yes — that’s an even more important refinement.
You don’t actually want “automatic growth.”
You want:
small stable slices
that MAY later connect
if reality proves they should
That’s a huge conceptual difference.
The earlier version still leaned slightly too much toward:
“everything becomes a system eventually.”
But most real work is:
tiny utility
→ useful
→ maybe another utility
→ maybe they connect later
→ maybe not
So the correct model is NOT:
seed
→ growth thread
→ inevitable platform
The correct model is:
slice
→ validated usefulness
→ optional adjacency discovery
→ optional composition
→ optional promotion
Very important distinction.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
10/48
The corrected philosophy
Wrong mental model
Every small thing is secretly a future platform.
That creates:
overengineering
fake architectures
premature abstractions
startup death spirals
enterprise theater
Correct mental model
Every slice should justify its own existence independently.
Only AFTER repeated evidence should:
slices connect
capabilities emerge
domains form
systems appear
The new rule
No growth without pressure.
Pressure means:
repeated pain
repeated reuse
repeated overlap
repeated workflow
repeated artifacts
repeated dependencies
Until then:
keep it tiny.
Your 3D printing example — corrected
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
11/48
Initial slice
Download images from model website.
That’s it.
Not:
Just:
YAML
model intelligence system
moderation platform
enrichment pipeline
safety architecture
slice:
id: slice.download_model_images
goal: >
Download model images locally.
ends_with:- images downloaded
nothing_more:
true
Then later
The user says:
I wish I could generate descriptions from these images.
That does NOT mean:
expand existing project
It means:
new adjacent slice candidate
New slice
YAML
slice:
id: slice.generate_model_description
goal: >
Generate a description from an image set.
depends_on:- artifact.image_set
not_yet:- part_of_system- capability- domain
That’s critical.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
12/48
The relationship exists ONLY because:
one slice produced an artifact
another slice consumes it
NOT because:
“we are now building a platform”
The missing concept: adjacency
Instead of “growth thread,” the better concept is:
YAML
adjacency:
from: slice.download_model_images
to: slice.generate_model_description
reason:
artifact_reuse
strength:
weak
Weak means:
related
but not yet promoted
not yet architectural
not yet organizational
Promotion must be earned
Only after repeated use:
download images
↓
generate descriptions
↓
deduplicate models
↓
tag categories
↓
moderate unsafe models
↓
all using same image artifact
NOW LENS can say:
There appears to be an emerging capability cluster.
NOT before.
New core principle
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
13/48
Slice-first ontology
The true root object is NOT:
domain
service
capability
initiative
system
The true root object is:
slice
Everything else is derived later.
Revised hierarchy
slice
↓
adjacency
↓
repeated pattern
↓
capability
↓
capability cluster
↓
domain
↓
system
NOT:
system
→ decompose downward
unless the user explicitly starts top-down.
This is the key duality
LENS must support BOTH:
Top-down mode
We know the system.
Help decompose it.
AND:
Bottom-up mode
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
14/48
We know one useful thing.
Help us avoid overbuilding.
The new architectural rule
Promotion thresholds
A concept cannot promote upward until evidence exists.
Example
One slice
download images
No capability yet.
Two slices sharing same artifact
download images
generate descriptions
Maybe adjacency.
Still no domain.
Five slices sharing same artifact
download images
generate descriptions
moderate unsafe models
deduplicate models
generate tags
Now:
YAML
emergent_capability:
id: capability.model_image_processing
confidence: medium
Still MAYBE not a domain.
Domain threshold
Only when:
multiple capabilities
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
15/48
stable ownership
repeated workflows
stable boundaries
architectural cohesion
appear together.
Then:
YAML
domain:
id: domain.model_intelligence
This solves over-modeling
One of the biggest risks identified in the redesign was:
Over-modeling
Teams may create structure too early
Reimagining LENS Framework
This revised model fixes that.
Revised commands
Instead of:
Bash
lens promote capability
You get:
Bash
lens detect adjacency
lens detect repetition
lens suggest promotion
NOT automatic promotion.
Example workflow
Day 1
Bash
lens slice new "download model images"
Day 5
Bash
lens slice new "generate descriptions from image sets"
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
16/48
LENS:
YAML
adjacency_detected:
shared_artifact:- image_set
recommendation:
keep_independent_for_now
Day 20
Several more slices emerge.
NOW:
YAML
pattern_detected:
repeated_artifact:- image_set
repeated_workflow:- ingest- analyze- classify
repeated_dependencies:- image metadata- listing metadata
recommendation:
consider capability promotion
This is MUCH safer
Because the framework now says:
structure should emerge from repeated pressure
not anticipation
That’s the actual missing insight.
The revised philosophy in one sentence
LENS should help teams discover structure only when reality repeatedly demands it.
Not sooner.
The final mental model
Top-down
known system
→ discover slices
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
17/48
Bottom-up
small slices
→ discover adjacencies
→ detect repeated pressure
→ optionally promote structure
Both coexist.
That’s the real breakthrough.
FE918A6A-9C8B-44A5-BA3B-698EBB58ECFD.jpeg
9DE101E2-CDB8-488C-A00C-88A848810800.jpeg
3D1C39C1-D3B0-49BA-9583-0FA8A6192E1B.jpeg
B21772B8-3094-4F27-8540-EA2E21E8C93F.jpeg
02D4E27A-2F4E-411C-8AB4-A42C8E4E455D.jpeg
DE9AA62A-42E5-44E6-94AE-8BB6FA79D533.jpeg
2B4C5FB2-D5C9-4E48-966B-1C79BF8D4C85.jpeg
D798E777-7B6A-4D01-B0E1-393FC9EF12B4.jpeg
07790C9F-3DB9-4DDD-BC8F-A61B825A3EBE.jpeg
A5374CD9-E6F4-45A4-B354-8ABF5A0AD74D.jpeg
OCR These and then integrate them into the design above for a final description of the system.
These screenshots actually push the design into its final form. They resolve a major tension that still
existed in the earlier drafts:
how do we support tiny slices without losing long-term coherence?
The answer emerging from the screenshots is:
The slice is the operational unit.
The landscape is the accumulated interpretation.
The graph is derived.
Promotion is optional.
Identity is stable.
Structure emerges only when pressure demands it.
That is the final synthesis.
The screenshots add five critical corrections to the earlier design.
Final LENS System Definition
LENS is a BMAD-native system-discovery, slice-orchestration,
and topology-evolution framework for complex software systems.
It is designed to support:- tiny isolated slices- evolving products- organic growth- enterprise-scale systems- long-lived knowledge- AI-assisted development- multi-team coordination
without requiring premature structure.
The biggest conceptual breakthrough from the screenshots
The screenshots finally clarify:
Features/slices are NOT the durable truth.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
18/48
This was the missing piece.
The screenshots repeatedly emphasize:
humans need stable places to read current truth
while:
features are archaeological records of work
That distinction changes everything.
Final Topology Model
Tree 1 — Slice Archive
Operational history.
Append-only.
Never reorganized.
This is NOT where humans learn the current system.
This is where work history lives.
docs/
slices/
download-model-images/
generate-model-description/
evidence-visible-to-teacher/
Each slice contains:
scratchpads
implementation notes
BMAD packets
stories
validation
Salmon signals
experiments
decisions during execution
Slices are immutable-ish operational records.
Tree 2 — Living Landscape
This is the CURRENT interpretation of reality.
This is what humans read.
This evolves over time.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
19/48
landscape/
capabilities/
domains/
systems/
journeys/
ledgers/
This is NOT derived from folder layout.
It is:
curated
promoted
synthesized
reorganizable
This is where:
capabilities emerge
domains form
systems become understandable
Tree 3 — Derived Graph
Machine-only projection.
NEVER authoritative.
The screenshots strongly reinforce this.
graph/
derived-map.yaml
relationship-index.yaml
traceability-index.yaml
Purpose:
AI traversal
dependency detection
impact analysis
automation
search
audits
traceability
Humans should NOT author here directly.
Final Mental Model
This is the final corrected architecture:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
20/48
Slices are reality.
Landscape is interpretation.
Graph is projection.
That’s the final model.
The final hierarchy
The screenshots helped reveal the correct hierarchy:
slice
↓
adjacency
↓
repeated pressure
↓
capability
↓
capability cluster
↓
domain
↓
program/system
NOT:
system
→ domain
→ service
→ feature
unless explicitly operating top-down.
The most important addition from the screenshots
Promotion is NOT automatic
This is crucial.
The screenshots repeatedly emphasize:
promotable topology
NOT:
mandatory topology
That changes the framework philosophy entirely.
Correct Promotion Model
Slice
A local useful thing.
May remain forever isolated.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
21/48
Example:
download model images
No capability.
No domain.
No architecture.
No promotion.
Adjacency
Two slices share:
artifacts
workflows
dependencies
meaning
But remain independent.
YAML
adjacency:
from: slice.download_model_images
to: slice.generate_model_description
strength: weak
Capability
ONLY when repeated pressure emerges.
Example:
download images
generate descriptions
detect duplicates
classify unsafe models
extract metadata
NOW:
YAML
capability:
id: capability.model_image_processing
Domain
ONLY when:
stable boundaries
stable ownership
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
22/48
stable workflows
stable semantics
stable coordination
emerge.
Stable identity over mutable location
This became MUCH clearer in the screenshots.
This is probably the most important technical rule.
IDs are identity.
Paths are addresses.
Meaning:
YAML
slice:
id: slice.download_model_images
is permanent.
But:
docs/slices/download-model-images/
may later move.
The ID survives.
This allows:
reorganization
promotion
merging
domain formation
capability restructuring
WITHOUT breaking references.
Final BMAD Relationship
The screenshots make the BMAD relationship much cleaner.
BMAD owns execution
PRD
UX
architecture
epics
stories
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
23/48
implementation
reviews
LENS owns coherence
discovery
relationships
slice orchestration
landscape evolution
traceability
promotion
impact analysis
knowledge topology
Final interaction model
Smallest possible flow
Bash
lens slice new "download model images"
lens prepare bmad
bmad-create-prd
bmad-create-architecture
bmad-create-epics-and-stories
bmad-dev-story
That’s it.
No system required.
No domain required.
No capability required.
Later
Another slice appears.
Bash
lens slice new "generate descriptions from image sets"
LENS notices:
YAML
adjacency:
shared_artifact:- image_set
recommendation:
keep_independent
No forced growth.
Months later
Repeated pressure emerges.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
24/48
NOW LENS says:
YAML
promotion_candidate:
type: capability
reason:
repeated_artifacts
repeated_workflows
repeated_dependencies
Still advisory.
Not automatic.
The biggest architectural correction
The screenshots kill the old idea that:
“folders ARE the architecture”
Now:
Folders are storage.
Topology is metadata.
Meaning is ledger-based.
That’s huge.
Final Source of Truth Model
The screenshots repeatedly reinforce:
The graph is NOT the source of truth.
Instead:
Source Truth
Control-repo files.
YAML
slice.yaml
capability.yaml
domain.yaml
journey.yaml
Projection
Derived graph.
Rebuildable.
Disposable.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
25/48
Final Relationship Lifecycle
The screenshots also finalize relationship maturity.
extracted
↓
hypothesized
↓
challenged
↓
reviewed
↓
promoted
↓
implemented
↓
validated
↓
superseded
This is extremely important for AI systems because it prevents hallucinated certainty.
The real role of Salmon
The screenshots clarified this too.
Salmon is NOT:
notifications
event bus
alerts
Salmon is:
recursive upstream consistency validation
Meaning:
implementation reveals truth
↓
truth may invalidate assumptions
↓
assumptions propagate upward
↓
landscape reconciles
↓
BMAD may correct-course
This is how:
tiny slices
evolving reality
long-lived systems
stay coherent.
Final design principle
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
26/48
The screenshots finally unify everything into one principle:
LENS should never force structure ahead of demonstrated need.
That’s the final philosophy.
Final one-sentence definition
LENS is a BMAD-native slice orchestration and evolving knowledge-topology framework
that lets systems emerge organically while preserving long-term coherence,
traceability, and machine-readable relationships.
Final concise model
Operational Truth
Slices
Human Current Truth
Landscape ledgers
Machine Traversal Truth
Derived graph
Evolution Mechanism
Promotion under pressure
Correction Mechanism
Salmon
Execution Engine
BMAD
That’s the final architecture.
integrate all concepts from the chat into one final description of lens both salmon and top down 
being supported.
Below is the final integrated description of LENS as it has evolved across the chat: a BMAD-native
module that supports both top-down system discovery and bottom-up slice-scale growth, with
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
27/48
Salmon as the upstream correction mechanism.
Final LENS Definition
LENS stands for:
Large-system
Exploration,
Navigation,
Slicing,
and validation framework
LENS is a BMAD-native module for discovering, decomposing, executing, validating, and continuously
reorganizing complex software work.
It is not a standalone product framework. It is not a replacement for BMAD. BMAD remains the
execution method: PRD, UX, architecture, epics, stories, implementation, code review, correction, and
retrospective. LENS wraps around BMAD as the system-understanding, slice-selection, relationship,
traceability, and adaptation layer. 
Pasted text
The simplest description is:
BMAD makes the work buildable.
LENS makes the work understandable, traceable, adaptable, and coherent.
Or even shorter:
LENS understands the system.
BMAD builds the slice.
LENS checks whether the built slice still matches reality.
Core Purpose
LENS helps teams move from either:
large ambiguous vision
or:
one tiny useful slice
into well-scoped BMAD execution without losing context, overbuilding too early, or allowing
downstream implementation discoveries to disappear.
It supports both mental models:
Top-down:
Known or suspected system
→ roles
→ outcomes
→ loops
→ journeys
→ slices
→ capabilities
→ BMAD execution
and:
Bottom-up:
Small useful slice
→ local artifact
→ optional adjacency
→ repeated pressure
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
28/48
→ optional capability
→ optional domain
→ optional system
→ BMAD execution only when needed
That dual support is the heart of the final design.
What LENS Is Not
LENS is not:
a PRD generator
a standalone app
a domain/service/feature organizer
a forced enterprise architecture tool
a system that assumes everything must grow
a replacement for BMAD
a tool that turns every small idea into a platform
LENS must not force structure before reality demands it.
The final principle is:
No growth without pressure.
Pressure means repeated evidence such as:
repeated artifact reuse
repeated workflow
repeated dependency
repeated risk
repeated ownership concern
repeated cross-slice coordination
repeated user journey
repeated implementation friction
Until pressure exists, a slice can remain a slice.
The Central Unit: Slice
The final LENS model makes slice the operational unit.
A slice is:
a small, testable, end-to-end unit of useful work
It may be:
tiny utility
workflow step
product journey segment
integration path
proof of concept
feature-sized implementation
A slice is allowed to exist without:
system
domain
service
capability
program
initiative
roadmap
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
29/48
This is what fixes the bottom-up case.
Example:
YAML
slice:
id: slice.download_model_images
goal: Download images from a 3D model website.
status: active
scope:
includes:- fetch model listing page- identify image URLs- download images locally- record source metadata
excludes:- model description generation- safety detection- moderation workflow- marketplace publishing- model intelligence platform
That slice is complete on its own.
It does not need to become anything else.
The Two Growth Modes
Mode 1: Top-Down LENS
Top-down mode is used when the user has a large ambiguous system idea.
Example input:
I want to build a platform for schools where students, teachers, coaches,
and leaders all work from the same learning improvement system.
LENS should not jump directly to a PRD.
Instead, it opens a discovery epoch:
capture raw thinking
→ extract hypotheses
→ challenge assumptions
→ identify roles
→ map outcomes
→ discover operating loops
→ map journeys
→ select one vertical slice
→ prepare BMAD packet
→ BMAD plans and builds
→ LENS validates and updates landscape
The uploaded framework emphasizes that LENS should not ask humans to define the whole product
upfront; it should capture raw thinking, extract candidate concepts, challenge assumptions, focus one
outcome, map one journey, slice one path, build a little, validate, update the living landscape, and
repeat. 
Reimagining LENS Framework
Top-down LENS produces artifacts like:
system thesis
role map
stakeholder map
outcome matrix
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
30/48
operating loop
journey map
slice roadmap
capability candidates
impact map
BMAD packet
Example top-down compression:
Broad system:
education improvement platform
Focused outcome:
teacher turns student evidence into instructional action
Journey:
evidence to teacher action
First slice:
evidence visible to teacher
BMAD packet:
build only evidence visibility, source metadata, access rules, and safe empty states
This matches the earlier NorthStar-style flow where a broad education platform idea becomes focused
through discovery, journey mapping, slicing, capability mapping, impact analysis, BMAD planning,
validation, and landscape update. 
Reimagining LENS Framework
Mode 2: Bottom-Up LENS
Bottom-up mode is used when the user only knows one useful thing.
Example input:
I want to download images from 3D printing model websites.
LENS should not assume this is a platform.
It should create a slice:
YAML
slice:
id: slice.download_model_images
status: active
goal: Download model listing images locally.
Later, if the user says:
Now I want to generate descriptions from those images.
LENS creates another slice:
YAML
slice:
id: slice.generate_model_description
goal: Generate a model description from downloaded images.
consumes:- artifact.model_image_set
LENS may detect an adjacency:
YAML
adjacency:
from: slice.download_model_images
to: slice.generate_model_description
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
31/48
_
_
reason: shared_artifact
shared_artifacts:- artifact.model_image_set
strength: weak
recommendation: keep_independent_for_now
No promotion yet.
Only after repeated pressure appears:
download images
generate descriptions
deduplicate models
tag model categories
detect unsafe content
review flagged models
can LENS suggest:
YAML
promotion_candidate:
type: capability
id: capability.model_image_processing
confidence: medium
reason:- repeated artifact reuse- repeated workflow- repeated classification need- repeated metadata dependency
This preserves the slice-scale mindset:
A slice may remain small forever.
A slice may connect later.
A slice may promote later.
Promotion is optional, explicit, and evidence-driven.
Final Ontology
LENS has a flexible ontology that supports both top-down and bottom-up work.
YAML
core_entities:
slice:
meaning: Smallest operational unit of useful, testable work.
artifact:
meaning: Something produced or consumed by a slice.
adjacency:
meaning: A weak relationship between slices, usually because they share artifacts, workflows, 
users, risks, or dependencies.
relationship:
meaning: A typed connection between entities with lifecycle, confidence, and provenance.
capability:
meaning: A durable system ability that emerges from repeated slice pressure.
journey:
meaning: An end-to-end path through which an outcome becomes real.
outcome:
meaning: A desired change for a user, role, business, system, or stakeholder.
role:
meaning: A human or system actor.
stakeholder:
meaning: A person or group with influence, constraints, or decision power.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
32/48
operating_loop:
meaning: A repeated cycle the product enables or improves.
domain:
meaning: A coherent area of product meaning, ownership, capability, or workflow.
service:
meaning: A technical/runtime boundary, derived later from architecture and implementation.
workstream:
meaning: A coordinated stream of work that may span slices, capabilities, services, domains, and 
teams.
program:
meaning: A larger coherent initiative or product area formed from multiple domains or capability 
clusters.
system:
meaning: The larger product-system when one is known or has emerged.
decision:
meaning: A durable choice that constrains future work.
assumption:
meaning: A belief used for planning that may be validated, invalidated, or superseded.
risk:
meaning: A possible harm to coherence, safety, delivery, trust, privacy, cost, or usability.
evidence:
meaning: Proof that a slice, journey, outcome, or relationship holds.
salmon_signal:
meaning: A downstream discovery that may need to update upstream truth.
auspex_status:
meaning: Read-only stakeholder visibility over current state, risk, evidence, freshness, and 
progress.
The key rule:
Feature, domain, service, and system are not mandatory planning roots.
They may appear later as landscape metadata.
The Final Knowledge Topology
The screenshots and prior design converge on a Two-Tree Model with a Derived Map. This is essential
because humans need stable places to read current truth, while machines need generated indexes and
graphs. The uploaded framework describes the Work Archive, Living Landscape, Derived Map, stable
IDs, metadata-based validity, Salmon, and Auspex as the sustainable knowledge topology for LENS.
Reimagining LENS Framework
Tree 1: Work Archive
The Work Archive preserves what happened.
It is append-only or mostly append-only.
It contains:
raw sessions
brainstorms
uploaded notes
whiteboards
slice runs
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
33/48
BMAD packets
stories
implementation evidence
validation results
discarded ideas
Salmon signals
historical decisions
superseded assumptions
Example:
_bmad-output/lens/archive/
capture/
extractions/
slices/
bmad-packets/
implementation-evidence/
validation-results/
salmon-signals/
A slice belongs here because it is operational history.
The archive answers:
What did we do?
Why did we do it?
What was known then?
What did this slice produce?
What did implementation reveal?
Tree 2: Living Landscape
The Living Landscape preserves current truth.
It is curated, reorganizable, and human-readable.
It contains:
system ledgers
program ledgers
domain ledgers
capability ledgers
service ledgers
journey ledgers
workstream ledgers
decision ledgers
risk ledgers
Example:
_bmad-output/lens/landscape/
systems/
programs/
domains/
capabilities/
services/
journeys/
workstreams/
decisions/
risks/
The landscape answers:
What does the system currently mean?
What capabilities exist?
What domains are forming?
What journeys are active?
What assumptions remain open?
What risks constrain work?
What has changed recently?
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
34/48
This is where humans go first.
Tree 3: Derived Map
The Derived Map is generated from archive and landscape metadata.
It is not source truth.
It is rebuildable.
Example:
_bmad-output/lens/graph/
derived-map.yaml
derived-map.json
relationship-index.yaml
traceability-index.yaml
freshness-index.yaml
warnings.yaml
The graph powers:
AI context
impact analysis
dependency detection
traceability
Auspex dashboards
BMAD packet generation
Doctor audits
relationship traversal
Salmon propagation
The final rule:
Archive records history.
Landscape records current truth.
Graph projects machine-readable relationships.
Or:
Slices are reality.
Landscape is interpretation.
Graph is projection.
Stable Identity Over Mutable Location
This is one of the most important architecture rules.
IDs are identity.
Paths are addresses.
A thing can move without changing identity.
Example:
YAML
capability:
id: capability.model_image_processing
current_path: _bmad-output/lens/landscape/capabilities/model-image-processing/
Later it may move under a domain:
YAML
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
35/48
capability:
id: capability.model_image_processing
current_path: _bmad-output/lens/landscape/domains/model-intelligence/capabilities/model-image
processing/
The ID survives.
This allows the topology to evolve without breaking references.
The uploaded topology redesign explicitly identifies stable identity over mutable location, promotable
topology, human-first consolidation, machine-derived projection, and upstream impact as core
architecture principles. 
Reimagining LENS Framework
Planning Artifact Validity
Planning artifacts are not valid because of branch placement.
They are valid because of metadata.
YAML
artifact:
id: artifact.prd.evidence_visible_to_teacher
type: bmad_prd
status: reviewed
validity: current
source_of_truth: false
planned_for:- slice.evidence_visible_to_teacher
Allowed statuses:
draft
reviewed
approved
blocked
superseded
archived
This avoids the old planning-branch trap where git topology pretends to be governance.
Relationship Lifecycle
Relationships are first-class.
They do not appear fully formed.
They mature.
raw
→ extracted
→ hypothesized
→ challenged
→ reviewed
→ promoted
→ planned
→ implemented
→ validated
→ superseded
→ archived
Example:
YAML
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
36/48
relationship:
  id: rel.outcome.teacher_evidence_action.realized_by.journey.evidence_to_teacher_action
  from: outcome.teacher_turns_evidence_into_action
  type: realized_by
  to: journey.evidence_to_teacher_action
  status: hypothesized
  confidence: medium
  discovered_from:
    - extraction.001
  review:
    status: human_review_needed
  promotion:
    landscape_status: not_promoted
  validation:
    implementation_status: not_validated
After review and first slice validation:
YAML
relationship:
  id: rel.outcome.teacher_evidence_action.realized_by.journey.evidence_to_teacher_action
  status: reviewed
  confidence: high
  promotion:
    landscape_status: promoted
  validation:
    implementation_status: partially_validated
    validated_by:
      - slice.evidence_visible_to_teacher
The relationship lifecycle and gate model were explicitly developed in the prior framework: relationships
move from extracted and hypothesized through review, promotion, planning, implementation,
validation, and supersession; they pass through discovery, challenge, promotion, BMAD,
implementation, Salmon, and validation gates. 
Reimagining LENS Framework
Relationship Types
YAML
relationship_types:
  - id: expresses
    example: system_thesis expresses system intent
  - id: serves
    example: outcome serves role
  - id: realized_by
    example: outcome realized_by journey
  - id: decomposed_into
    example: journey decomposed_into slice
  - id: produces_artifact
    example: slice.download_model_images produces_artifact artifact.model_image_set
  - id: consumes_artifact
    example: slice.generate_model_description consumes_artifact artifact.model_image_set
  - id: adjacent_to
    example: slice.download_model_images adjacent_to slice.generate_model_description
  - id: requires
    example: slice.evidence_visible_to_teacher requires capability.role_based_visibility
  - id: participates_in
    example: capability.evidence_artifact_model participates_in domain.evidence_and_portfolios
  - id: implemented_by
    example: capability.role_based_visibility implemented_by service.identity_access
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 37/48
- id: planned_by
example: slice.evidence_visible_to_teacher planned_by artifact.bmad_prd- id: decomposed_by
example: artifact.bmad_prd decomposed_by artifact.bmad_epics- id: implemented_by_story
example: slice.evidence_visible_to_teacher implemented_by_story story.evidence_detail_view- id: validated_by
example: acceptance_evidence validates slice- id: impacted_by
example: domain.identity_access impacted_by salmon.001- id: supersedes
example: decision.support_relationship_policy supersedes decision.teacher_roster_only_policy
Promotion Model
Promotion is explicit.
Promotion is not automatic.
Promotion means a local thing becomes part of the Living Landscape.
The promotion ladder is:
slice
→ adjacency
→ repeated pattern
→ capability candidate
→ capability
→ capability cluster
→ domain
→ program
→ system
A slice does not have to climb the ladder.
Promotion requires evidence.
YAML
promotion_gate:
candidate: capability.model_image_processing
promoted_from:- slice.download_model_images- slice.generate_model_description- slice.detect_duplicate_models- slice.detect_unsafe_models
evidence:- repeated use of artifact.model_image_set- repeated image processing workflow- repeated metadata dependency- repeated classification concern
recommendation: consider_promotion
automatic: false
This directly addresses the over-modeling risk: the prior design called out that teams may create too
much structure too early, and the mitigation is to keep depth optional and add layers only when
justified. 
Reimagining LENS Framework
BMAD Integration
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
38/48
LENS is installed as a BMAD module.
Skills should use BMAD-native naming:
bmad-lens-help
bmad-lens-intake
bmad-lens-discover
bmad-lens-capture
bmad-lens-synthesize
bmad-lens-context-check
bmad-lens-research-plan
bmad-lens-map-system
bmad-lens-map-outcomes
bmad-lens-map-loops
bmad-lens-map-journeys
bmad-lens-slice-journey
bmad-lens-map-capabilities
bmad-lens-analyze-impact
bmad-lens-promote-landscape
bmad-lens-prepare-bmad
bmad-lens-sync-bmad
bmad-lens-guard-story
bmad-lens-validate-slice
bmad-lens-validate-journey
bmad-lens-validate-outcome
bmad-lens-salmon
bmad-lens-doctor
bmad-lens-auspex
BMAD owns:
analysis
planning
solutioning
implementation
story creation
dev story
code review
correct-course
retrospective
LENS owns:
discovery
context sufficiency
relationship modeling
slice selection
impact analysis
traceability
landscape evolution
Salmon propagation
Doctor audits
Auspex visibility
The prior prompt explicitly defined this relationship: LENS discovers and maintains the product-system,
BMAD plans and executes build work, and LENS validates whether built work still serves the intended
outcome. 
Pasted text
LENS Layers
YAML
layers:
0_bmad_core_runtime:
owns:- BMAD workflows- PRD- UX- architecture
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
39/48
      - epics
      - stories
      - implementation
      - code review
      - correct-course
  1_capture_layer:
    owns:
      - raw sessions
      - brainstorming
      - stakeholder notes
      - whiteboards
      - uploaded docs
      - existing artifacts
  2_extraction_layer:
    owns:
      - candidate concepts
      - roles
      - outcomes
      - workflows
      - risks
      - assumptions
      - unknowns
  3_intent_layer:
    owns:
      - system thesis
      - role map
      - stakeholder map
      - outcome matrix
      - operating loops
      - principles
      - constraints
  4_journey_layer:
    owns:
      - journey catalog
      - journey maps
      - journey steps
      - cross-role paths
  5_slice_layer:
    owns:
      - selected slice
      - slice scope
      - acceptance evidence
      - explicit out-of-scope
      - slice roadmap
  6_capability_landscape_layer:
    owns:
      - promoted capabilities
      - domains
      - services
      - workstreams
      - ledgers
  7_derived_map_layer:
    owns:
      - generated graph
      - relationship index
      - traceability index
      - freshness index
  8_bmad_bridge_layer:
    owns:
      - focused BMAD packet
      - project-context.md
      - PRD input
      - UX input
      - architecture input
      - epic/story input
  9_implementation_guard_layer:
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 40/48
    owns:
      - story traceability
      - scope guard
      - acceptance evidence guard
      - risk guard
  10_salmon_correction_layer:
    owns:
      - upstream impact detection
      - recursive consistency traversal
      - correct-course recommendation
      - landscape reconciliation
  11_auspex_visibility_layer:
    owns:
      - read-only stakeholder status
      - evidence state
      - freshness
      - risks
      - blockers
      - BMAD progress
The layered architecture from the prior work defines these same layers and clarifies that LENS structures
complexity, BMAD formalizes and executes delivery, and LENS watches whether execution still matches
system intent. 
Reimagining LENS Framework
BMAD Packet
LENS never dumps the whole messy system into BMAD.
It prepares a focused packet for the current slice.
YAML
bmad_packet:
  id: bmad_packet.slice.evidence_visible_to_teacher
  active_slice: slice.evidence_visible_to_teacher
  include:
    - system_thesis_if_available
    - focused_outcome_if_available
    - journey_context_if_available
    - slice_scope
    - explicit_out_of_scope
    - required_capabilities
    - risks
    - decisions_needed
    - acceptance_evidence
  exclude:
    - adjacent future slices
    - unvalidated system assumptions
    - speculative platform architecture
    - unpromoted capability clusters
For bottom-up slices, the packet may be extremely small:
YAML
bmad_packet:
  active_slice: slice.download_model_images
  include:
    - slice_goal
    - current artifact expectations
    - acceptance evidence
    - explicit exclusions
  exclude:
    - model description
    - content moderation
    - safety classification
    - platform architecture
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 41/48
For top-down systems, it may include more context:
YAML
bmad_packet:
active_slice: slice.evidence_visible_to_teacher
include:- system thesis- focused outcome- journey- slice scope- capabilities- risks- decisions- acceptance evidence
exclude:- AI recommendations- coaching dashboards- district analytics- family portal
The prior design emphasized that LENS prepares a focused BMAD packet and BMAD then creates formal
planning artifacts such as PRD, architecture, epics, and stories; LENS later syncs the BMAD artifacts back
into its traceability graph. 
Reimagining LENS Framework
Implementation Guard
Once BMAD creates stories, LENS guards them.
A BMAD story must trace to:
system, if known
→ outcome, if known
→ journey, if known
→ slice
→ capability, if promoted
→ acceptance evidence
For small bottom-up work, the trace may be:
slice
→ artifact
→ acceptance evidence
For top-down work, the trace may be:
system
→ role
→ outcome
→ journey
→ slice
→ capability
→ story
→ evidence
Example guard result:
YAML
guard_result:
story: story.evidence_detail_view
status: pass
lens_trace:
system: system.learning_improvement_platform
outcome: outcome.teacher_turns_evidence_into_action
journey: journey.evidence_to_teacher_action
slice: slice.evidence_visible_to_teacher
capabilities:- capability.evidence_artifact_model
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
42/48
- capability.role_based_visibility
checks:- name: story_traces_to_active_slice
status: pass- name: story_does_not_expand_scope
status: pass- name: acceptance_evidence_present
status: pass- name: privacy_boundary_acknowledged
status: pass
LENS guards BMAD stories for traceability, scope boundaries, and outcome evidence while BMAD owns
story-by-story execution. 
Pasted text
Salmon
Salmon is the upstream correction mechanism.
It exists because implementation reveals reality.
Sometimes a slice discovers something that invalidates upstream assumptions.
Examples:
teacher access cannot be determined from roster alone
downloaded model images are not enough without listing metadata
safety classification requires human review
a service boundary is wrong
a journey is incomplete
a capability does not exist yet
a privacy rule is missing
a related workstream is impacted
Salmon lets that discovery swim upstream.
story
→ slice
→ journey
→ outcome
→ capability
→ domain
→ program
→ system
Salmon does not replace BMAD correct-course.
Salmon decides:
Is this local?
Does it update landscape truth?
Does it require BMAD correct-course?
Does it invalidate a slice?
Does it change a journey?
Does it affect architecture?
Does it impact another workstream?
BMAD correct-course handles formal replanning when scope, architecture, or assumptions materially
change.
The prior framework defines Salmon as the upstream-change layer: implementation discoveries
propagate from story to slice, journey, outcome, capability/domain/service ledger, and possibly BMAD
plan; if needed, LENS recommends BMAD correct-course and then syncs the result back.
Reimagining LENS Framework
Example Salmon signal:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
43/48
YAML
salmon_signal:
id: salmon.001
raised_from: story.evidence_detail_view
severity: high
discovery: >
Teacher access cannot be determined from class roster alone.
Some support staff need limited visibility without being assigned classroom teachers.
impacted_nodes:- slice.evidence_visible_to_teacher- capability.teacher_student_relationship- capability.role_based_visibility- decision.teacher_access_policy- domain.identity_and_access
recommended_action:
type: correct_course
reason: >
The current acceptance criteria and architecture assumptions depend on
an incomplete access model.
bmad_action:
suggested_workflow: bmad-correct-course
Salmon is not just an alert.
It is:
recursive upstream consistency validation
Doctor
Doctor audits the topology.
It finds:
orphaned slices
missing IDs
duplicate IDs
broken references
stale ledgers
missing source refs
unpromoted completed slices
BMAD artifacts not synced
stories without LENS traceability
derived map drift
relationship contradictions
unresolved high-severity decisions
Example:
YAML
doctor_warning:
id: warning.unresolved_decision
severity: high
message: >
slice.evidence_visible_to_teacher requires decision.teacher_access_policy
before implementation can proceed safely.
Doctor is the consistency safety net.
Auspex
Auspex is the read-only visibility plane.
It reads the Derived Map.
It is not source truth.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
44/48
It shows:
system status
active slices
active journeys
risks
open decisions
BMAD progress
artifact freshness
validation evidence
Salmon signals
blocked work
Example:
YAML
auspex_status:
active_focus:
slice: slice.evidence_visible_to_teacher
bmad_status:
prd: in_progress
architecture: not_started
epics_and_stories: not_started
risks:- risk.privacy_boundary- risk.evidence_definition_ambiguity
open_decisions:- decision.teacher_access_policy
salmon_signals:
open: 0
resolved: 0
The prior design positions Auspex as a stakeholder visibility layer that shows system status, active
journeys and slices, risks, blockers, freshness, validation evidence, BMAD links, and source traceability
without requiring stakeholders to navigate the repo. 
Reimagining LENS Framework
Top-Down Example
User:
I want a school learning platform where students, teachers, coaches,
and leaders all work from shared evidence.
LENS flow:
Bash
bmad-lens-intake
bmad-lens-discover
bmad-lens-synthesize
bmad-lens-context-check
bmad-lens-map-system
bmad-lens-map-outcomes
bmad-lens-map-loops
bmad-lens-map-journeys
bmad-lens-slice-journey
bmad-lens-map-capabilities
bmad-lens-analyze-impact
bmad-lens-prepare-bmad
Result:
YAML
system:
id: system.learning_improvement_platform
status: hypothesized
focused_outcome:
id: outcome.teacher_turns_evidence_into_action
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
45/48
_
_
_
_
journey:
id: journey.evidence_to_teacher_action
selected_slice:
id: slice.evidence_visible_to_teacher
explicit_out_of_scope:- AI interpretation- coaching dashboards- leadership analytics- family portal
Then BMAD:
Bash
bmad-product-brief
bmad-create-prd
bmad-create-ux-design
bmad-create-architecture
bmad-create-epics-and-stories
bmad-check-implementation-readiness
Then LENS:
Bash
bmad-lens-sync-bmad
bmad-lens-guard-story
bmad-lens-validate-slice
bmad-lens-doctor
bmad-lens-auspex
Bottom-Up Example
User:
I want to download images from a 3D printing model website.
LENS flow:
Bash
bmad-lens-slice-new "download model images"
bmad-lens-prepare-bmad --slice slice.download_model_images
BMAD builds the tiny slice.
Later:
User:
Now I want to generate descriptions from those images.
LENS:
Bash
bmad-lens-slice-new "generate model descriptions"
bmad-lens-detect-adjacency
Result:
YAML
adjacency:
from: slice.download_model_images
to: slice.generate_model_description
shared_artifact:- artifact.model_image_set
recommendation: keep_independent_for_now
Later, repeated pressure emerges.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
46/48
Bash
bmad-lens-detect-repetition
bmad-lens-suggest-promotion
Result:
YAML
promotion_candidate:
type: capability
id: capability.model_image_processing
confidence: medium
automatic: false
The system grows only when it has earned growth.
Final Command Surface
bmad-lens-help
# Slice-scale
bmad-lens-slice-new
bmad-lens-slice-frame
bmad-lens-slice-scope
bmad-lens-prepare-bmad
# Discovery
bmad-lens-intake
bmad-lens-discover
bmad-lens-capture
bmad-lens-synthesize
bmad-lens-context-check
bmad-lens-research-plan
# Top-down modeling
bmad-lens-map-system
bmad-lens-map-outcomes
bmad-lens-map-loops
bmad-lens-map-journeys
bmad-lens-slice-journey
# Bottom-up evolution
bmad-lens-detect-adjacency
bmad-lens-detect-repetition
bmad-lens-suggest-promotion
bmad-lens-promote-landscape
# Impact and bridge
bmad-lens-map-capabilities
bmad-lens-analyze-impact
bmad-lens-prepare-bmad
bmad-lens-sync-bmad
# Implementation guard
bmad-lens-guard-story
bmad-lens-validate-slice
bmad-lens-validate-journey
bmad-lens-validate-outcome
# Adaptation
bmad-lens-salmon
bmad-lens-doctor
bmad-lens-auspex
Final Architecture in One Diagram
┌──────────────────────────┐
│          BMAD             │
│ PRD / UX / Architecture   │
Printed using 
│
│
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
47/48
│ Epics / Stories / Code    │
└─────────────▲────────────┘
│
focused BMAD packet
│
┌──────────────────────────────────────┴──────────────────────────────────────┐
│                                   LENS                                      │
│                                                                              │
│  Top-down mode:                                                              │
│  system → role → outcome → loop → journey → slice                            │
│                                                                              │
│  Bottom-up mode:                                                             │
│  slice → artifact → adjacency → repeated pressure → optional promotion        │
│                                                                              │
│  Knowledge topology:                                                         │
│  Work Archive  +  Living Landscape  +  Derived Map                           │
│                                                                              │
│  Adaptation:                                                                 │
│  Salmon upstream correction + Doctor audits + Auspex visibility              │
└──────────────────────────────────────────────────────────────────────────────┘
Final LENS Philosophy
Do not force the whole system up front.
Do not let small work lose context.
Do not promote structure before reality demands it.
Do not let implementation discoveries die downstream.
Do not let BMAD plan more than the selected slice needs.
Do not make the graph source truth.
Do not make folders identity.
Do not treat AI hypotheses as facts.
Instead:
Start at the right scale.
Preserve what happened.
Curate what is currently true.
Generate maps for machines.
Promote only under pressure.
Correct upstream through Salmon.
Use BMAD to build focused slices.
Validate, learn, and update the landscape.
Final One-Sentence Description
LENS is a BMAD-native slice orchestration and evolving knowledge-topology module
that supports both top-down complex-system discovery and bottom-up slice-scale growth,
using Work Archive, Living Landscape, Derived Map, explicit promotion, Doctor audits,
Auspex visibility, and Salmon upstream correction to keep software work coherent as it is built.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
48/48