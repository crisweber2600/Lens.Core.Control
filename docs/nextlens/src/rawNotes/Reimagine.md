Reimagining LENS Framework
Lens.Core.Release-develop.zip
Zip Archive
I’m trying to reimagine lens. Right now it has domain service feature. But domain and service had 
no real context. So I’m reimagining it to have a product or maybe goals or outcomes? This would 
be an intense brainstorming phase and session outlining the idea and goals involved. Then that 
can be analyzed for distinct features or services needed. And the outcomes it needs to have. Then 
user journey and integration. This is where an outcome of the feature is broken down into a 
specific journey across all layers. The goal is to have vertical slicing of a user journey from end to 
end and have those guided by ai and have the impact on the codebase be analyzed and taken into 
account and if it changes a related workstream then take that into account too. https://docs.bmad
method.org//llms-full.txt
I’ll treat this as a redesign pass for LENS, not just renaming “domain/service/feature.” I’ll ground it in
BMAD’s workflow concepts, then map your new idea into a concrete operating model: product goals →
outcomes → journeys → vertical slices → codebase/workstream impact.
The key diagnosis already looks clear: domain/service/feature is a storage and routing model, but you’re
trying to build a thinking model. The new unit should probably start from “why/value/outcome,” then
derive features, services, repos, and stories afterward.
Yes — I think the breakthrough is:
LENS should not start with domain → service → feature.
That is implementation topology. It should start with product intent → desired outcome → user
journey → vertical slice → codebase impact.
Right now, domain/service/feature makes LENS ask:
“Where does this work live?”
The reimagined LENS should ask:
“What outcome are we trying to create, who experiences it, what journey changes, and what
systems must participate?”
BMAD already supports this direction because its flow starts with
analysis/brainstorming/research/product brief, then planning, then architecture, then implementation.
It explicitly frames Phase 1 as brainstorming, research, product brief, or PRFAQ before PRD and
architecture. BMAD Method+1
The new mental model
I would reframe LENS around this hierarchy:
Product / Capability Area
↓
Goal / Outcome
↓
User Journey
↓
Journey Slice
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
1/88
↓
Feature / Capability Change
↓
Impact Map
↓
Services / Repos / Code / Tests / Workstreams
So instead of:
domain: payments
service: checkout
feature: saved-card
You get:
product_area: checkout experience
outcome: reduce payment failure recovery time
journey: buyer retries failed payment and completes order
slice: failed card → alternate payment → order confirmation
impacted_systems:- frontend checkout- payment orchestration API- card token service- order service- notification service
related_workstreams:- wallet redesign- fraud rules migration- payment observability upgrade
That is much closer to how real enterprise work behaves.
The big design shift
I would make Outcome or Initiative the new top-level LENS object.
Not feature.
A feature is too small and too implementation-shaped. An outcome gives the AI enough context to
reason.
Possible names:
initiative
outcome
product_goal
workstream
journey_lens
My recommendation:
initiative
contains outcomes
contains journeys
contains slices
produces features/stories/tasks
Use initiative as the durable planning container, and slice as the implementation unit.
New LENS 2.0 object model
1. initiative.yaml
This replaces the current over-reliance on feature.yaml.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
2/88
YAML
initiative_id: checkout-payment-recovery
name: Checkout Payment Recovery
product_area: Checkout
problem: Buyers who hit payment failure often abandon instead of recovering.
business_goal: Increase successful checkout completion after payment failure.
primary_users:- buyer- customer support agent- payment operations analyst
desired_outcomes:- id: recover-failed-payment
description: Buyer can recover from a failed payment without restarting checkout.
success_metrics:- payment failure recovery completion rate increases- support contacts for failed checkout decrease- average retry time decreases
non_goals:- replacing payment provider- redesigning full checkout- changing fraud policy engine
constraints:- must preserve PCI boundaries- must not create duplicate orders- must work with existing payment tokenization model
related_workstreams:- wallet redesign- fraud migration- checkout observability
current_phase: outcome-discovery
2. journey.yaml
This becomes the bridge between product thinking and implementation.
YAML
journey_id: buyer-recovers-failed-payment
outcome_id: recover-failed-payment
actor: buyer
trigger: card payment fails during checkout
start_state: cart ready, order not placed, payment failed
end_state: order placed successfully with alternate payment method
steps:- user sees payment failure reason- user chooses another saved card or new payment method- payment is retried- order is confirmed- receipt is sent- payment event is observable to operations
touchpoints:- web checkout UI- mobile checkout UI- payment API- order API- notification system- analytics pipeline
acceptance_evidence:- e2e journey test passes- payment retry integration test passes- no duplicate order created- telemetry event emitted- rollback path documented
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
3/88
3. slice.yaml
This is the new vertical-slice unit.
YAML
slice_id: failed-card-to-alternate-card
journey_id: buyer-recovers-failed-payment
slice_goal: Buyer can retry payment with a different saved card after failure.
vertical_path:
ui:- checkout payment failure panel- saved payment selector
api:- retry payment endpoint- payment method validation
domain_logic:- prevent duplicate order creation- preserve failed attempt history
data:- payment_attempt table- order payment status
observability:- payment_retry_started- payment_retry_succeeded- payment_retry_failed
tests:- e2e checkout retry path- payment retry contract test- duplicate order regression test
impacted_repos:- checkout-web- payment-orchestrator- order-service
related_slices:- failed-card-to-new-card- failed-card-to-wallet-payment
risk_level: high
reason: payment retry touches order state, payment state, and duplicate prevention.
This is the core of what you described: a user journey broken vertically across every layer, with AI
guided impact analysis.
Where domain and service still belong
Do not delete domain/service.
Demote them.
They should become implementation metadata, not the planning root.
Old:
domain/service/feature determines the work
New:
outcome/journey/slice determines the work
domain/service/repo explains where the work lands
So domain and service become part of the impact map:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
4/88
YAML
impact_map:
domains:- checkout- payments- orders
services:- checkout-web- payment-orchestrator- order-service
contracts:- PaymentRetryRequest- OrderPaymentState
shared_dependencies:- auth/session- feature flags- telemetry schema
That fixes the problem you identified: domain/service had no real context because they were being
asked to carry product meaning. They should carry system ownership and routing meaning.
Proposed LENS 2.0 lifecycle
Your current uploaded LENS lifecycle has:
preplan → businessplan → techplan → finalizeplan → dev → complete
I would evolve it into:
discover
↓
frame-outcome
↓
map-journey
↓
slice-journey
↓
analyze-impact
↓
plan-implementation
↓
execute-slice
↓
validate-outcome
↓
complete
Mapped to BMAD:
LENS 2.0 phase
discover
Purpose
intense brainstorming, raw idea exploration
frame-outcome
map-journey
slice-journey
analyze-impact
plan
implementation
execute-slice
define users, goals, metrics, non-goals
define end-to-end user/system journey
find vertical slices
scan repos, services, contracts, related
workstreams
BMAD alignment
BMAD brainstorming/research
product brief / PRFAQ / PRD input
UX + product planning
PRD-to-epic bridge
architecture + project context
generate epics/stories/tests
implement one vertical slice
validate-outcome
Printed using 
verify journey and outcome evidence
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
BMAD epics/stories
dev story
code review + readiness +
retrospective
5/88
This is very compatible with BMAD because BMAD already separates analysis, planning, solutioning, and
implementation; it also creates epics/stories after architecture so technical decisions can shape the work
breakdown. BMAD Method+1
The killer feature: “journey impact graph”
This is what would make LENS valuable beyond BMAD.
For every proposed slice, LENS should generate:
Journey Impact Graph
Example:
Buyer retries failed payment
↓
Checkout UI
↓
Payment Orchestrator API
↓
Payment Provider Adapter
↓
Order State Machine
↓
Receipt Notification
↓
Analytics / Ops Dashboard
Then AI analyzes:
What files likely change?
What services are involved?
What contracts are touched?
What existing stories/workstreams conflict?
What tests prove the journey?
What observability proves it in production?
What feature flags or rollout controls are needed?
This becomes the bridge from human product intent to AI-safe code execution.
Related workstream detection
This part is important.
You said:
if it changes a related workstream then take that into account too
That should become a formal LENS artifact:
YAML
workstream_impact:
directly_impacted:- checkout-payment-recovery
possibly_conflicting:- wallet-redesign- payment-provider-migration
shared_files:- packages/payment-contracts/src/retry.ts- apps/checkout/src/payment/PaymentFailurePanel.tsx
shared_contracts:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
6/88
- PaymentAttempt- PaymentMethod- OrderPaymentStatus
decision_needed:- Should retry behavior belong to checkout or payment orchestration?- Should wallet redesign own saved-card selector changes?
Then before implementation, LENS should force a gate:
Related workstream review required?- No: continue- Yes: create dependency note / block / split slice / update plan
This is how LENS becomes enterprise-safe.
New command model
I would stop making /new-feature the main entry point.
New commands:
/new-initiative
/discover-outcome
/map-journey
/slice-journey
/analyze-impact
/plan-slice
/dev-slice
/validate-journey
Backward-compatible commands can remain:
/new-feature → creates a slice under an initiative
/new-service → implementation inventory only
/new-domain → governance/ownership inventory only
The important change:
/new-feature should no longer be the starting point for meaningful work.
It should be generated after the outcome and journey are understood.
The strongest version of the idea
I would describe reimagined LENS like this:
LENS is an outcome-to-code orchestration layer for AI-assisted enterprise development. It
starts with product goals and user outcomes, converts them into end-to-end user journeys,
slices those journeys vertically across UI/API/domain/data/integration layers, analyzes the
codebase and related workstreams for impact, then guides BMAD-style planning and
implementation with persistent context, governance, and review gates.
That positioning is much stronger than:
LENS manages domain/service/feature context.
The latter sounds like a scaffolding tool.
The former sounds like the missing enterprise AI development layer.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
7/88
My recommended next design decision
Use this structure:
Product Area
→ Initiative
→ Outcome
→ Journey
→ Slice
→ Feature/Story/Task
→ Repo/Service/File/Test
And make the central LENS promise:
Every story must trace back to:
1. an outcome,
2. a journey step,
3. a vertical slice,
4. an impact map,
5. acceptance evidence.
That gives you the governance backbone you were missing.
BMAD gives you the phase machinery: brainstorming, PRD, architecture, epics/stories, implementation
readiness, story implementation, and review. LENS should become the enterprise context layer that
decides what the work means across products, journeys, systems, and workstreams before BMAD
executes it. BMAD also emphasizes project context for existing codebases so agents follow established
patterns instead of generic assumptions, which fits perfectly with LENS impact analysis. BMAD Method
What about a system as complex as this. https://happy-flower-0aa290510.7.azurestaticapps.net/ 
read the raw html so you can see the full context
I’ll inspect the page as source/context first, then translate the complexity into a LENS model: what the
“product/outcome/journey/slice” hierarchy would look like for a real multi-system app instead of a toy
example.
The url is a large product that needs to be built  
This is exactly the kind of product where domain → service → feature breaks down.
NorthStarET is not “a feature app.” It is an educational operating system: many roles, one coherent
system, role-based dashboards, shared continuous-improvement loops, AI/learning intelligence,
evidence portfolios, coaching workflows, micro-credentials, and district-level insight. The page explicitly
frames it as “one platform, many roles, one coherent system,” serving students, families, teachers,
happy-flower
coaches, school leaders, and district leaders. 
0aa290510.7.azurestaticapps.net
So for something this large, LENS should not model the system as:
domain → service → feature
It should model it as:
system → role → outcome → journey → loop step → vertical slice → capability → service/repo/code
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
8/88
The better LENS model for NorthStarET
For this product, the root object is not a feature. It is a system intent.
YAML
system:
name: NorthStarET
type: educational operating system
promise: one coherent system across every layer of learning
operating_model:
roles:- student- family- teacher- coach- school_leader- district_leader
pillars:- workspace- pathways- coach
improvement_loop:- assess- set_goals- learn- apply- evidence- improve
intelligence_layer:
name: Joey
responsibilities:- administer individualized assessments- analyze learning artifacts- identify strengths and needs- align evidence to standards- generate instructional guidance
That structure comes directly from the page: NorthStarET describes the same continuous improvement
loop for every role, with steps for Assess, Set Goals, Learn, Apply, Evidence, and Improve. 
happy-flower
0aa290510.7.azurestaticapps.
It
also describes Joey as the intelligence engine that administers assessments, analyzes artifacts, finds
happy-flower
patterns, aligns evidence to standards, and turns that into guidance. 
The key LENS insight
0aa290510.7.azurestaticapps.net
For NorthStarET, the continuous improvement loop is the product spine.
Not the services.
Not the database.
Not the dashboards.
Not even Joey.
The spine is:
Assess → Set Goals → Learn → Apply → Evidence → Improve
Everything else hangs off that loop.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
9/88
The page says every audience experiences the same coherent system, expressed through a dashboard
happy-flower
built for their daily work. 
0aa290510.7.azurestaticapps.net
That means LENS needs to understand that a “feature” is often not
one UI screen or one API. A real feature is a journey through the loop across multiple roles.
Example vertical slice
Take this outcome:
Teachers can turn student evidence into the next instructional action.
That sounds like one feature, but it is really an end-to-end journey:
Student completes assessment or learning task
↓
Artifact/evidence is captured
↓
Joey analyzes strengths, needs, and standards alignment
↓
Teacher sees instructional guidance
↓
Teacher sets or adjusts goals
↓
Teacher applies a strategy
↓
Evidence is stored in portfolio
↓
Coach sees patterns for coaching cycle
↓
School/district leader sees aggregate implementation health
That one slice touches many capabilities:
YAML
slice:
name: evidence-to-instructional-action
outcome: teacher can act on student learning evidence quickly
roles:
primary:- teacher- student
secondary:- coach- school_leader- district_leader
loop_steps:- assess- evidence- improve- apply
product_pillars:- workspace- coach- pathways
capabilities:- assessment_capture- evidence_portfolio- standards_alignment- joey_analysis- teacher_dashboard- goal_recommendation- coaching_visibility- leadership_indicators
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
10/88
  likely_services:
    - identity_and_role_access
    - roster_org_model
    - assessment_service
    - evidence_service
    - standards_service
    - joey_intelligence_service
    - goal_plan_service
    - dashboard_bff
    - analytics_indicators_service
This is why your instinct is right: features should be derived after outcome and journey analysis.
How LENS should handle a product this complex
I would create a new top-level LENS artifact called:
system-map.yaml
Not domain map. Not service map.
For NorthStarET, it could look like this:
YAML
system_id: northstar-et
system_name: NorthStarET
system_type: role_based_learning_improvement_platform
north_star_outcome: >
  Every learner and every adult role has the tools, learning path,
  evidence, and guidance needed to improve continuously.
roles:
  student:
    core_outcomes:
      - understand my goals
      - complete learning tasks
      - see my progress
      - build ownership habits
  family:
    core_outcomes:
      - understand child progress
      - support organization and growth
  teacher:
    core_outcomes:
      - assess student needs
      - receive instructional guidance
      - monitor progress
      - collect evidence
      - connect practice to professional learning
  coach:
    core_outcomes:
      - identify coaching needs
      - support teacher implementation
      - monitor evidence of practice
  school_leader:
    core_outcomes:
      - see implementation health
      - identify support needs
      - align school priorities
  district_leader:
    core_outcomes:
      - see system-wide coherence
      - monitor leading indicators
      - support long-term improvement
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 11/88
pillars:
workspace:
purpose: do the work
pathways:
purpose: grow the work
coach:
purpose: guide the work
continuous_improvement_loop:- assess- set_goals- learn- apply- evidence- improve
intelligence_layer:
name: Joey
purpose: translate artifacts and activity into guidance, patterns, and system insight
The page’s “What’s Inside” section maps almost perfectly to those three pillars: Workspace for role-based
daily work, Pathways for student learning and adult micro-credentials, and Coach for next-best actions
happy-flower
and guided routines. 
0aa290510.7.azurestaticapps.net
LENS should use a matrix, not a tree
A tree is too weak for this product.
This is not:
Product
→ Domain
→ Service
→ Feature
It is a matrix:
Role × Pillar × Loop Step × Outcome
Example:
Role
Student
Student
Teacher
Teacher
Coach
Leader
Pillar
Workspace
Pathways
Workspace
Coach
Coach
Loop step
Assess
Set Goals
Evidence
Outcome
Complete learning task
Understand personal goals
Review student artifacts
Improve
Improve
Workspace
Improve
Then LENS generates vertical slices from intersections.
Example:
Teacher × Coach × Evidence/Improve
becomes:
Receive next instructional action
Identify teacher support needs
See implementation health
Slice: Teacher receives Joey-generated guidance from evidence
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
12/88
Example:
Student × Family × Goals/Progress
becomes:
Slice: Family sees child goal progress and suggested support
Example:
Coach × School Leader × Implementation Health
becomes:
Slice: Coach cycle evidence rolls up to school-level support indicators
That is the right granularity.
The services should emerge later
Eventually, yes, you will probably need services like:
identity / auth / RBAC
organization / district / school / classroom model
roster / SIS integration
assessment engine
evidence portfolio
standards alignment
goal and learning plan service
pathway / credential service
Joey intelligence service
coaching workflow service
analytics / leading indicators
notification / nudging service
dashboard BFF / role experience API
But LENS should not begin there.
Those are implementation consequences.
The product should first be sliced by outcomes:
Can the student complete and understand the assessment?
Can the teacher convert evidence into action?
Can the coach turn teacher evidence into support?
Can the leader see implementation health without micromanaging?
Can adult learning connect back to student needs?
That fits the product’s stated promise: stronger instructional coherence, higher adult capacity, greater
student ownership, clearer system insight, and sustained improvement. 
How BMAD fits this
happy-flower
0aa290510.7.azurestaticapps.net
BMAD is still the workflow engine, but LENS becomes the product-context compiler.
BMAD’s documented flow has Analysis, Planning, Solutioning, and Implementation phases. Analysis
includes brainstorming, research, product brief, and PRFAQ; Planning creates PRD/spec; Solutioning
creates architecture; then Implementation builds epic by epic and story by story. BMAD Method BMAD also
says architecture should come before epics and stories because architecture decisions affect how work
is broken down. BMAD Method
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
13/88
For NorthStarET, I would use LENS before and during BMAD like this:
LENS Discover System
→ BMAD Brainstorming / Product Brief
LENS Role-Outcome Matrix
→ BMAD PRD
LENS Journey Maps
→ BMAD UX + PRD journeys
LENS Capability Graph
→ BMAD Architecture
LENS Slice Planner
→ BMAD Epics and Stories
LENS Impact Graph
→ BMAD Create Story / Dev Story / Code Review
LENS Outcome Validation
→ BMAD Readiness Check / Test Architecture / Retrospective
So BMAD remains the build method. LENS becomes the thing that prevents BMAD from losing the
product meaning.
The reimagined LENS object hierarchy
For a product like this, I would define the hierarchy as:
System
→ Product Area
→ Role
→ Outcome
→ Journey
→ Journey Step
→ Vertical Slice
→ Capability
→ Service / Repo / Code / Test
For NorthStarET:
System: NorthStarET
Product Area: Continuous Improvement Loop
Role: Teacher
Outcome: Use student evidence to improve instruction
Journey: Review evidence and receive Joey guidance
Slice: Evidence artifact → standards alignment → next action
Capabilities:- artifact ingestion- standards mapping- Joey analysis- teacher dashboard- goal adjustment- coaching visibility
The “big product” command set
For a complex system, I would not use /new-feature.
I would create these:
/lens-new-system
/lens-map-roles
/lens-map-outcomes
/lens-map-journeys
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
14/88
/lens-slice-journey
/lens-map-capabilities
/lens-analyze-impact
/lens-plan-product-area
/lens-plan-slice
/lens-validate-outcome
Then /new-feature becomes a generated artifact, not the starting point.
The most important design rule
For NorthStarET-scale products, every implementation story should trace back to:
1. System promise
2. Role
3. Outcome
4. Journey
5. Loop step
6. Vertical slice
7. Capability
8. Service/repo/code
9. Evidence of success
That gives you exactly what the old domain/service/feature model lacked: meaning.
The old model says:
This story belongs to the assessment service.
The new model says:
This story helps a teacher turn student evidence into instructional action during the Evidence → 
Improve loop, using Joey analysis, with coaching and leadership visibility preserved.
That is the level of context an AI agent needs to safely build a product like this.
My strongest recommendation
Rename the central LENS concept from feature to slice.
A feature is ambiguous.
A slice means:
a complete, testable, end-to-end path through the product that delivers part of an outcome
For NorthStarET, the product should be built slice by slice:
Slice 1: Student completes self-assessment
Slice 2: Teacher sees needs summary
Slice 3: Teacher sets goal from evidence
Slice 4: Joey recommends instructional action
Slice 5: Evidence enters e-portfolio
Slice 6: Coach sees teacher implementation signal
Slice 7: Leader sees aggregate system-health indicator
Slice 8: Adult pathway is recommended from student need pattern
That sequence is much more buildable than trying to design “the assessment domain,” “the coaching
service,” or “the dashboard feature” in isolation.
The reimagined LENS should be an outcome-to-architecture compiler:
raw product vision
→ role/outcome matrix
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
15/88
→ journey graph
→ vertical slices
→ capability map
→ service boundaries
→ BMAD PRD/architecture/epics/stories
→ code impact and validation
That is the right model for something as complex as NorthStarET.
Focus on what changes to lens would need to happen for a system this complex to handle and step 
me through the user journey of using this new lens version as an addon to bmad.  
For a NorthStarET-scale system, LENS needs to stop being a feature lifecycle manager and become a
product-system workbench that wraps BMAD.
The current LENS release you uploaded is still centered on:
new-domain → new-service → new-feature
feature.yaml
preplan → businessplan → techplan → finalizeplan → dev → complete
That works for normal engineering work. It is not enough for a platform like NorthStarET, where the
product has many roles, one continuous improvement loop, three product pillars, and an intelligence
engine called Joey. The page frames the system as “one platform, many roles, one coherent system,”
with students, families, teachers, coaches, school leaders, and district leaders all using role-based
dashboards. It also defines the repeated loop as Assess → Set Goals → Learn → Apply → Evidence →
happy-flower
Improve. 
0aa290510.7.azurestaticapps.net
So the main change is:
LENS v1: Where does this feature live?
LENS v2: What outcome/journey/slice are we building, and what systems must participate?
1. The core LENS object needs to change
Replace feature.yaml as the center of gravity.
Keep it for backward compatibility, but introduce these new artifacts:
system.yaml
product-area.yaml
outcome.yaml
journey.yaml
slice.yaml
capability-map.yaml
impact-map.yaml
workstream-map.yaml
The new root should be:
System → Product Area → Outcome → Journey → Slice → Story → Code
For NorthStarET:
YAML
system_id: northstar-et
system_type: educational-operating-system
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
16/88
roles:- student- family- teacher- coach- school_leader- district_leader
pillars:- workspace- pathways- coach
improvement_loop:- assess- set_goals- learn- apply- evidence- improve
intelligence_layer:
name: Joey
responsibilities:- administer individualized assessments- analyze learning artifacts- identify strength and need patterns- align evidence to standards- generate instructional guidance
NorthStarET’s own page already gives LENS this structure: Workspace, Pathways, and Coach are the
three pillars, while Joey is the internal learning intelligence engine that analyzes artifacts, identifies
patterns, aligns evidence to standards, and organizes guidance. 
happy-flower
0aa290510.7.azurestaticapps.net
2. Domain/service/feature becomes implementation metadata
You do not delete domain/service/feature.
You demote them.
Old:
YAML
domain: learning
service: assessment
feature: student-self-assessment
New:
YAML
slice_id: student-self-assessment-to-goal-recommendation
outcome:
role: student
desired_change: student understands current needs and receives a next goal
journey:
start: student begins assessment
end: student sees recommended goal and next learning path
loop_steps:- assess- set_goals
capabilities:- assessment_capture
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
17/88
- standards_alignment- joey_analysis- goal_recommendation- student_dashboard
implementation_impact:
domains:- learning- assessment- goals- intelligence
services:- assessment-service- evidence-service- joey-service- pathway-service- dashboard-bff
That single change fixes the conceptual problem: domain/service no longer pretends to explain why
the work matters.
It only explains where the work lands.
3. LENS lifecycle should wrap BMAD, not replace it
BMAD already has the right high-level phase model: Analysis, Planning, Solutioning, and
Implementation. Its Analysis phase includes brainstorming, research, product brief, and PRFAQ;
Planning creates PRD/spec; Solutioning creates architecture and then epics/stories; Implementation
builds story by story. BMAD Method
So LENS should become a layer above BMAD:
LENS: decide what the product system means
BMAD: produce the formal artifacts and execution workflow
LENS: keep those artifacts tied to outcomes, journeys, slices, impact, and workstreams
Current LENS phases:
preplan → businessplan → techplan → finalizeplan → dev → complete
New LENS phases:
system-discovery
↓
outcome-framing
↓
journey-mapping
↓
slice-planning
↓
capability-mapping
↓
impact-analysis
↓
bmad-planning
↓
slice-implementation
↓
outcome-validation
Then map them to BMAD:
New LENS phase
system-discovery
outcome-framing
BMAD workflow it wraps
bmad-brainstorming, bmad-product-brief, research
product brief / PRFAQ / PRD input
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
18/88
PRD journeys + UX design
BMAD workflow it wraps
New LENS phase
journey-mapping
slice-planning
capability-mapping
impact-analysis
bmad-planning
slice
implementation
outcome-validation
PRD scope + epic candidates
architecture input
architecture + project context
bmad-create-prd, bmad-create-architecture, bmad-create-epics-and
stories
bmad-create-story, bmad-dev-story, bmad-code-review
readiness check, QA, retrospective
BMAD specifically recommends creating epics/stories after architecture because architecture decisions
affect how work should be broken down. That lines up perfectly with LENS generating slices first, then
letting architecture reshape the implementation breakdown. BMAD Method
4. New command surface
The current command surface is too feature-shaped.
Current:
/new-domain
/new-service
/new-feature
/preplan
/businessplan
/techplan
/finalizeplan
/dev
New:
/lens-new-system
/lens-import-product-context
/lens-map-roles
/lens-map-outcomes
/lens-map-journeys
/lens-slice-journey
/lens-map-capabilities
/lens-analyze-impact
/lens-detect-workstreams
/lens-plan-with-bmad
/lens-start-slice
/lens-validate-outcome
Keep old commands as aliases:
/new-feature → /lens-start-slice
/new-domain → /lens-register-implementation-domain
/new-service → /lens-register-implementation-service
That lets LENS remain backward-compatible while making the new workflow outcome-first.
5. New LENS agents
Your current agent set can evolve cleanly:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
19/88
Product-system router
New role
Current idea
Compass
Scout
Scribe
Tracey
Casey
New: Mapper
New: Slicer
New: Impact
Repo + product-context discovery
Governance and artifact keeper
State, lineage, and traceability
Git/branch/PR orchestration
Role/outcome/journey mapper
Vertical slice designer
Codebase/workstream impact analyzer
New: Validator
Outcome evidence and acceptance validator
For a NorthStarET-scale product, Mapper, Slicer, and Impact are the missing agents.
Without them, LENS can create features but cannot reason about a cross-role platform.
6. The new state model
Instead of only active feature state:
YAML
active_feature: learning-assessment-student-self-assessment
phase: businessplan
Use:
YAML
active_system: northstar-et
active_product_area: continuous-improvement-loop
active_outcome: student-understands-needs-and-next-goal
active_journey: student-assessment-to-goal-pathway
active_slice: initial-assessment-to-goal-recommendation
active_bmad_phase: planning
active_lens_phase: slice-planning
And every slice should carry traceability:
YAML
traceability:
system: northstar-et
role: student
pillar: pathways
loop_steps:- assess- set_goals
outcome: student-understands-needs-and-next-goal
journey: student-assessment-to-goal-pathway
slice: initial-assessment-to-goal-recommendation
bmad_artifacts:- product-brief.md- prd.md- architecture.md- epics-and-stories.md
This is how you prevent BMAD from creating generic stories disconnected from the product’s real
operating model.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
20/88
7. User journey: using new LENS as a BMAD add-on
Here is the actual user experience I would design.
Step 1 — Install BMAD and LENS
The user installs BMAD normally, then installs LENS as a custom module/add-on. BMAD supports
custom agents, workflows, and modules through BMad Builder and custom module installation, so LENS
fits the extension model rather than needing to fork BMAD. BMAD Method
Bash
npx bmad-method install
npx bmad-method install --custom-source ./lens-work
The project now has:
_bmad/
_bmad-output/
TargetProjects/
lens.core/
lens-governance/
Step 2 — Start LENS
User runs:
@lens
LENS does not ask for a domain/service/feature.
It asks:
What system are we building or extending?
User says:
NorthStarET
Then:
/lens-import-product-context https://happy-flower-0aa290510.7.azurestaticapps.net/
LENS reads the product context and creates:
systems/northstar-et/system.yaml
systems/northstar-et/source-context.md
systems/northstar-et/product-language.md
It extracts:
roles
pillars
continuous improvement loop
Joey intelligence responsibilities
impact promises
Step 3 — System discovery
User runs:
/lens-map-system
LENS produces:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
21/88
system-map.md
role-map.yaml
pillar-map.yaml
loop-map.yaml
intelligence-map.yaml
For NorthStarET, this means:
Roles:
student, family, teacher, coach, school leader, district leader
Pillars:
Workspace, Pathways, Coach
Loop:
Assess, Set Goals, Learn, Apply, Evidence, Improve
Intelligence:
Joey
This becomes the system spine.
Step 4 — Outcome framing
User runs:
/lens-map-outcomes
LENS creates a matrix:
Role × Pillar × Loop Step × Outcome
Example:
Role
Student
Student
Teacher
Teacher
Coach
Pillar
Pathways
Pathways
Workspace
Coach
Loop step
Assess
Set Goals
Evidence
Outcome
Student understands current strengths and needs
Student receives a personal learning goal
Teacher sees student evidence in usable form
Improve
Coach
District Leader
Workspace
Improve
Improve
Teacher receives next instructional action
Coach sees patterns across teacher practice
District sees system health without micromanagement
LENS then asks BMAD to support the product framing:
Run bmad-product-brief using the system map and outcome matrix.
Step 5 — Journey mapping
User selects an outcome:
Teacher receives next instructional action from student evidence.
User runs:
/lens-map-journey teacher-evidence-to-instructional-action
LENS creates:
YAML
journey_id: teacher-evidence-to-instructional-action
primary_role: teacher
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
22/88
supporting_roles:- student- coach- school_leader
start_state: student evidence exists
end_state: teacher has a next instructional action and coach visibility exists
steps:- student completes assessment or task- evidence is captured- Joey analyzes artifact- evidence is aligned to standards- teacher sees summary- teacher accepts, adjusts, or rejects guidance- goal/pathway is updated- coach sees implementation signal- leader sees aggregate indicator
This is where LENS becomes stronger than BMAD alone. BMAD can create a PRD, but LENS preserves the
journey as a first-class object.
Step 6 — Slice the journey
User runs:
/lens-slice-journey teacher-evidence-to-instructional-action
LENS proposes vertical slices:
Slice 1: Artifact capture → teacher evidence inbox
Slice 2: Evidence inbox → standards alignment
Slice 3: Standards alignment → Joey guidance
Slice 4: Joey guidance → teacher action decision
Slice 5: Teacher action → coaching visibility
Slice 6: Coaching signal → school/district indicator
Each slice must be end-to-end and testable.
For example:
YAML
slice_id: evidence-to-joey-teacher-guidance
start: teacher opens student evidence
end: teacher sees Joey-generated instructional guidance
vertical_path:
ui:- teacher dashboard- evidence detail panel- guidance card
api:- evidence query- joey analysis request- standards alignment lookup
data:- student artifact- evidence metadata- standards mapping- recommendation record
intelligence:- Joey analysis prompt- confidence / rationale- human override path
observability:- evidence_viewed- guidance_generated- guidance_accepted- guidance_overridden
tests:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
23/88
- e2e teacher reviews evidence- contract test for Joey response- regression test for missing standards
Step 7 — Capability mapping
User runs:
/lens-map-capabilities evidence-to-joey-teacher-guidance
LENS derives capabilities:
identity/RBAC
roster and classroom model
evidence portfolio
assessment/artifact ingestion
standards alignment
Joey intelligence
teacher dashboard
goal/pathway service
coaching workflow
analytics/indicators
This is where domain/service finally re-enters.
LENS says:
These are not product goals. These are implementation capabilities.
Step 8 — Codebase and workstream impact
User runs:
/lens-analyze-impact evidence-to-joey-teacher-guidance
LENS scans:
TargetProjects/
repo-inventory.yaml
existing PRDs
existing architecture docs
existing slice files
open workstreams
service ownership
contracts
events
shared schemas
It produces:
YAML
impact_map:
likely_repos:- northstar-web- northstar-api- joey-service- evidence-service- standards-service
shared_contracts:- EvidenceArtifact- StandardsAlignment- JoeyGuidanceResult- TeacherActionDecision
related_workstreams:- student-assessment-engine- adult-microcredential-pathways- leadership-indicators-dashboard
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
24/88
risks:- Joey guidance may create unreviewed instructional recommendations- standards alignment must be explainable- role visibility must avoid exposing student-level data incorrectly- coaching visibility must not become teacher surveillance
required_gates:- privacy review- human-in-the-loop decision rule- audit trail- role-based access test- journey-level e2e test
This is the “enterprise LENS” moment.
It does not just say “change assessment-service.”
It says:
This slice touches student evidence, teacher guidance, Joey intelligence, coaching visibility, and 
leadership indicators.
Step 9 — Hand off to BMAD planning
User runs:
/lens-plan-with-bmad evidence-to-joey-teacher-guidance
LENS now wraps BMAD:
bmad-create-prd
bmad-create-ux-design
bmad-create-architecture
bmad-create-epics-and-stories
bmad-check-implementation-readiness
But it injects LENS artifacts as required context:
system-map.yaml
outcome.yaml
journey.yaml
slice.yaml
capability-map.yaml
impact-map.yaml
workstream-map.yaml
constitution.md
project-context.md
BMAD already supports project-context.md to keep agents aligned with technical preferences, existing
patterns, and implementation rules. LENS should generate and maintain a richer version of that context
for each system/slice. BMAD Method
Step 10 — Finalize plan
User runs:
/lens-finalize-slice-plan
LENS checks:
Does every story trace to a slice?
Does every slice trace to an outcome?
Does every impacted repo have ownership?
Does every related workstream have a decision?
Does every Joey/intelligence touchpoint have human-review rules?
Does every journey have validation evidence?
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
25/88
If yes, LENS allows BMAD story execution.
If no, it blocks and says:
This is not implementation-ready.
Step 11 — Implement slice
User runs:
/lens-start-slice evidence-to-joey-teacher-guidance
LENS creates branch/PR topology around the slice, not the old feature.
slice/evidence-to-joey-teacher-guidance/plan
slice/evidence-to-joey-teacher-guidance/dev
slice/evidence-to-joey-teacher-guidance/review
Then for each story:
/lens-create-story
Which wraps:
bmad-create-story
Then:
/lens-dev-story
Which wraps:
bmad-dev-story
Then:
/lens-code-review
Which wraps:
bmad-code-review
BMAD’s build cycle is story creation, story implementation, and code review; LENS should not replace
that. It should make sure each story is born from a journey slice and carries the right impact context.
BMAD Method
Step 12 — Validate outcome, not just code
After implementation, user runs:
/lens-validate-outcome evidence-to-joey-teacher-guidance
LENS checks:
Can the teacher complete the journey?
Was evidence captured?
Was Joey guidance generated?
Was the standards alignment explainable?
Was teacher choice preserved?
Was coach visibility updated correctly?
Was leadership aggregation protected?
Were privacy boundaries respected?
Were telemetry events emitted?
The output is not just:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
26/88
Tests passed.
It is:
Outcome evidence passed.
Journey evidence passed.
Code evidence passed.
Workstream impact reviewed.
That is what makes it safe for complex platforms.
The most important structural change
The new LENS should make this rule non-negotiable:
No story may exist unless it traces to:
system → role → outcome → journey → slice → capability → impact map → acceptance evidence
For NorthStarET, a story should never say only:
Add Joey recommendation endpoint.
It should say:
As part of the Teacher Evidence → Instructional Action journey,
within the Evidence → Improve loop,
for the outcome “teacher can turn student evidence into next action,”
implement the Joey guidance endpoint so the teacher dashboard can display explainable instructional 
guidance,
while preserving student privacy, teacher agency, coach visibility, and leadership aggregate-only 
insight.
That is the real LENS upgrade.
What I would change first in your actual LENS module
Start with these concrete changes:
1. Rename core unit:
feature.yaml → work-item.yaml or slice.yaml
2. Add new governance folders:
systems/
outcomes/
journeys/
slices/
capabilities/
workstreams/
3. Add new commands:
/lens-new-system
/lens-map-outcomes
/lens-map-journey
/lens-slice-journey
/lens-analyze-impact
/lens-plan-with-bmad
/lens-validate-outcome
4. Keep old commands:
/new-feature becomes compatibility wrapper around /lens-start-slice
5. Update lifecycle.yaml:
Add system-discovery, outcome-framing, journey-mapping, slice-planning, capability-mapping, 
impact-analysis, bmad-planning, slice-implementation, outcome-validation
6. Update state:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
27/88
active_feature becomes active_system + active_outcome + active_journey + active_slice
7. Update impact analysis:
It must detect related workstreams, not only target repos
8. Update BMAD bridge:
Every BMAD workflow receives the active slice bundle as required context
The simplest way to describe the redesign:
Old LENS = feature-aware BMAD workbench.
New LENS = outcome/journey/slice-aware BMAD orchestration layer for enterprise systems.
That is the right shape for a product as complex as NorthStarET.
Reposition as a net new framework built purposely for large complex projects like Northstar. Do 
not limit yourself by lens current state
I’ll treat this as a clean-sheet framework design for NorthStar-class products: not “LENS v2,” but a new
methodology that happens to integrate with BMAD as its execution engine.
Reposition it as a net-new framework, not as “LENS but better.”
I would frame it as:
Constellation Framework
An AI-native product-system framework for designing and building large, complex, multi-role
platforms.
BMAD remains the execution engine. Constellation becomes the system-thinking layer above it.
BMAD is already designed to move from analysis → planning → solutioning → implementation, with
brainstorming, PRD, architecture, epics, stories, and code review workflows. It also supports custom
modules and workflows, which makes it a good host for something larger. BMAD Method
But NorthStar is bigger than a normal product backlog. The page describes it as “one platform, many
roles, one coherent system,” serving students, families, teachers, coaches, school leaders, and district
happy-flower
leaders through role-based dashboards. 
0aa290510.7.azurestaticapps.net
features. It has to start from system coherence.
The new positioning
Old idea:
That means the framework cannot start from
LENS helps AI understand domain, service, and feature context.
New idea:
Constellation helps AI turn a complex product-system vision into coherent outcomes, journeys, 
vertical slices, architecture, workstreams, and BMAD-executable stories.
The core promise:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
28/88
From system vision → role outcomes → journey graph → vertical slices → capability map → architecture 
→ BMAD stories → validated product outcomes.
For a product like NorthStar, the framework must understand:
Roles
Pillars
Improvement loops
AI/intelligence behavior
Evidence flows
Dashboards
Adult learning
Student learning
Coaching cycles
Leadership visibility
Privacy boundaries
Implementation workstreams
NorthStar’s public description gives the framework a clear spine: the platform is built around a
continuous improvement loop of Assess → Set Goals → Learn → Apply → Evidence → Improve.
happy-flower
0aa290510.7.azurestaticapps.net
It also has three major pillars: Workspace, Pathways, and Coach. 
happy-flower
Joey is the
system intelligence layer that administers assessments, analyzes artifacts, identifies patterns, aligns
0aa290510.7.azurestaticapps.net
happy-flower
evidence to standards, and turns that into guidance. 
That is not a feature list.
That is a product operating model.
0aa290510.7.azurestaticapps.net
What Constellation adds that BMAD does not own
BMAD should not be responsible for understanding a whole educational operating system by itself.
BMAD is the structured build method.
Constellation should own the layer before BMAD:
What is this system?
Who does it serve?
What outcomes matter?
How do roles interact?
What journeys exist?
What evidence moves through the system?
What capabilities are needed?
What risks exist?
What workstreams collide?
What vertical slices should be built first?
Then BMAD takes over for:
PRD
UX design
architecture
epics
stories
implementation
code review
readiness checks
BMAD’s own docs say its Method and Enterprise tracks are intended for products, platforms, complex
features, compliance, multi-tenant systems, PRD, architecture, UX, and story-by-story implementation.
BMAD Method Constellation would make BMAD better at the pre-BMAD system decomposition required for
NorthStar-scale work.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
29/88
Core concepts
1. System
The whole product ecosystem.
YAML
system:
name: NorthStarET
category: educational_operating_system
promise: one coherent platform for every layer of learning
users:- student- family- teacher- coach- school_leader- district_leader
2. Operating loop
The repeated improvement cycle that governs the whole system.
YAML
operating_loop:- assess- set_goals- learn- apply- evidence- improve
This matters because every feature should be placed somewhere in the loop.
3. Role-outcome matrix
Instead of writing features, Constellation maps outcomes by role.
Role × Loop Step × Outcome
Example:
Role
Student
Student
Teacher
Teacher
Coach
Leader
Loop step
Assess
Set Goals
Evidence
Improve
Improve
Outcome
Understand my current strengths and needs
Know what I am working toward next
See student work in an actionable way
Receive next instructional action
See patterns that guide coaching cycles
Improve
4. Journey graph
See implementation health without micromanaging
A journey is not a screen flow. It is an end-to-end path through the system.
Example:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
30/88
Student completes assessment
→ Joey analyzes artifact
→ standards alignment is created
→ teacher sees evidence
→ teacher gets guidance
→ teacher adjusts goal or plan
→ coach sees implementation signal
→ leader sees aggregate system health
This is the level where NorthStar becomes buildable.
5. Vertical slice
A slice is the smallest end-to-end path that proves part of the system works.
YAML
slice:
name: evidence-to-teacher-guidance
outcome: teacher can turn student evidence into a next instructional action
starts_with: student evidence exists
ends_with: teacher sees explainable guidance
includes:- student artifact- evidence storage- standards alignment- Joey analysis- teacher dashboard- audit trail- acceptance evidence
6. Capability map
Capabilities are derived from journeys.
Not guessed up front.
assessment capture
evidence portfolio
standards alignment
Joey intelligence
role dashboard
goal planning
coaching workflow
leadership indicators
identity/RBAC
privacy/audit
7. Workstream graph
This is essential for large projects.
Every slice must ask:
What other workstreams does this touch?
What contracts are shared?
What teams are affected?
What data is exposed?
What assumptions become dangerous?
What decisions must be made before coding?
For NorthStar, a Joey guidance slice might touch student evidence, adult micro-credentials, standards
alignment, dashboards, coaching cycles, and leadership analytics. Treating that as “just a feature” would
be unsafe.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
31/88
The framework stack
I would structure Constellation like this:
Constellation Framework
1. System Discovery
2. Outcome Modeling
3. Journey Mapping
4. Vertical Slice Design
5. Capability Derivation
6. Workstream and Impact Analysis
7. BMAD Planning Bridge
8. BMAD Execution Bridge
9. Outcome Validation
10. System Learning Loop
BMAD fits inside steps 7 and 8.
Constellation decides what should be built and why.
BMAD plans and executes how it gets built.
Constellation validates whether the system outcome was achieved.
User journey using Constellation with BMAD
Step 1: Create a system workspace
User starts:
constellation new-system NorthStarET
The framework asks for source material:
Product page
pitch deck
notes
raw brainstorm
strategy docs
existing repo
existing BMAD artifacts
User provides the NorthStar page.
Constellation extracts:
system promise
roles
pillars
improvement loop
Joey intelligence layer
impact claims
Output:
system-thesis.md
system-map.yaml
source-context.md
product-language.md
Step 2: Build the role-outcome matrix
Command:
constellation map-outcomes
The framework generates:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
32/88
student outcomes
family outcomes
teacher outcomes
coach outcomes
school leader outcomes
district leader outcomes
For each role, it maps outcomes to the improvement loop.
Output:
role-outcome-matrix.md
role-outcome-matrix.yaml
This prevents the product from becoming a pile of dashboards.
Step 3: Identify flagship journeys
Command:
constellation discover-journeys
The framework proposes the core product journeys.
For NorthStar, examples:
Student assessment → personal goal → learning pathway
Student evidence → teacher guidance → instructional action
Teacher practice → coach signal → coaching cycle
Student need pattern → adult micro-credential recommendation
Classroom implementation → school health indicator
School patterns → district support decision
Output:
journey-catalog.md
journeys/*.yaml
Step 4: Choose the first vertical slice
Command:
constellation slice-journey teacher-evidence-to-instructional-action
The framework breaks a large journey into buildable slices:
Slice 1: Student artifact captured and visible to teacher
Slice 2: Artifact aligned to standard
Slice 3: Joey generates teacher guidance
Slice 4: Teacher accepts, adjusts, or rejects guidance
Slice 5: Coach sees aggregate implementation signal
Slice 6: Leader sees protected system-health indicator
The framework recommends the first slice based on:
value
risk
dependency
technical unknowns
demo potential
learning potential
privacy sensitivity
Output:
slices/evidence-visible-to-teacher.yaml
slices/evidence-to-joey-guidance.yaml
slice-roadmap.md
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
33/88
Step 5: Derive capabilities
Command:
constellation map-capabilities evidence-to-joey-guidance
The framework derives the system capabilities needed:
identity/RBAC
student roster model
evidence artifact model
standards taxonomy
Joey analysis pipeline
teacher dashboard
recommendation explanation
teacher decision capture
audit/event logging
Output:
capability-map.yaml
capability-boundaries.md
This is where architecture begins to emerge naturally.
Step 6: Run impact and workstream analysis
Command:
constellation analyze-impact evidence-to-joey-guidance
The framework checks:
Which capabilities are touched?
Which data objects are shared?
Which teams or workstreams are affected?
Which risks are created?
Which policies are triggered?
Which tests prove the slice?
What must be true before BMAD planning starts?
Output:
impact-map.md
workstream-graph.yaml
risk-register.md
decision-log.md
This is one of the biggest differentiators.
A normal AI coding workflow says:
Build the Joey guidance feature.
Constellation says:
This slice changes the relationship between student evidence, teacher agency, AI-generated guidance, 
coaching visibility, and leadership insight. Here are the contracts, risks, privacy boundaries, and 
related workstreams.
That is what a NorthStar-class project needs.
Step 7: Generate BMAD-ready planning packet
Command:
constellation prepare-bmad evidence-to-joey-guidance
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
34/88
The framework creates a packet for BMAD:
system-thesis.md
role-outcome-matrix.md
journey.yaml
slice.yaml
capability-map.yaml
impact-map.md
risk-register.md
decision-log.md
acceptance-evidence.md
Then it instructs BMAD:
Run:
bmad-product-brief
bmad-create-prd
bmad-create-ux-design
bmad-create-architecture
bmad-create-epics-and-stories
bmad-check-implementation-readiness
BMAD’s documented flow already supports product brief, PRD, UX, architecture, epics/stories, readiness
checks, sprint planning, story creation, implementation, and code review. BMAD Method
Constellation’s job is to make sure BMAD receives the right context.
Step 8: BMAD creates the build plan
BMAD produces:
PRD.md
UX design
architecture.md
epics
stories
implementation readiness check
But every BMAD artifact must include Constellation traceability:
System
Role
Outcome
Journey
Slice
Capability
Risk
Acceptance evidence
A story should not just say:
Create teacher guidance card.
It should say:
For the Teacher Evidence → Instructional Action journey,
within the Evidence → Improve loop,
implement the guidance card so a teacher can view explainable Joey-generated next actions from 
student evidence,
while preserving teacher agency, student privacy, and auditability.
That is the new standard.
Step 9: Execute through BMAD, guarded by Constellation
During implementation, BMAD runs:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
35/88
bmad-sprint-planning
bmad-create-story
bmad-dev-story
bmad-code-review
Constellation wraps each story with checks:
Does this story still match the slice?
Did the implementation cross a role boundary?
Did it create a new data exposure?
Did it affect another workstream?
Did it weaken the outcome?
Did it produce the required evidence?
This is where the framework becomes a safety layer for AI coding.
Step 10: Validate outcome, not just code
Command:
constellation validate-outcome evidence-to-joey-guidance
The framework checks:
Can the teacher complete the journey?
Was evidence captured?
Was guidance generated?
Was guidance explainable?
Could the teacher accept, adjust, or reject it?
Was an audit trail created?
Were student privacy boundaries respected?
Did telemetry prove the journey?
Did related workstreams stay coherent?
Output:
outcome-validation.md
journey-test-results.md
system-learning-log.md
This matters because passing unit tests is not enough for a system like NorthStar. The product promise
is coherence, capacity, insight, student ownership, and sustained improvement. 
framework must validate those outcomes, not just implementation tasks.
The major framework artifacts
/constellation
/systems
northstar-et/
system-thesis.md
system-map.yaml
operating-loop.yaml
role-map.yaml
pillar-map.yaml
intelligence-map.yaml
/outcomes
role-outcome-matrix.md
outcomes/*.yaml
/journeys
journey-catalog.md
journeys/*.yaml
/slices
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
happy-flower
0aa290510.7.azurestaticapps.net
The
HTML to PDF API.
36/88
slice-roadmap.md
slices/*.yaml
/capabilities
capability-map.yaml
capability-boundaries.md
/impact
impact-map.md
workstream-graph.yaml
risk-register.md
decision-log.md
/bmad-bridge
bmad-context-packet.md
prd-input.md
architecture-input.md
story-context-template.md
/validation
acceptance-evidence.md
journey-tests.md
outcome-validation.md
The new agent roles
Constellation should have its own agents, separate from BMAD’s PM, architect, UX, dev, and QA agents.
System Cartographer
Maps the product-system, roles, loops, pillars, and intelligence layers.
Outcome Strategist
Turns vision into role-specific outcomes.
Journey Architect
Builds end-to-end journey graphs across roles and systems.
Slice Designer
Finds thin vertical slices that prove the product.
Capability Mapper
Derives capabilities and service boundaries from journeys.
Impact Analyst
Finds codebase, data, privacy, workstream, and dependency impact.
BMAD Liaison
Packages Constellation context into BMAD workflows.
Outcome Validator
Checks whether the built system actually satisfies the journey and outcome.
BMAD agents then execute the formal software-delivery flow.
The command surface
constellation new-system
constellation import-context
constellation map-system
constellation map-outcomes
constellation discover-journeys
constellation slice-journey
constellation map-capabilities
constellation analyze-impact
constellation prepare-bmad
constellation sync-bmad-artifacts
constellation guard-story
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
37/88
constellation validate-outcome
constellation correct-course
BMAD commands remain BMAD commands.
Constellation does not replace them.
It makes them safer and more coherent for complex systems.
The key design principle
For a NorthStar-class system:
A feature is not allowed to exist unless it traces back to a role, outcome, journey, slice, and 
system promise.
That is the heart of the new framework.
No orphan stories.
No isolated features.
No dashboard sprawl.
No AI-generated code without product-system context.
No architecture disconnected from user journeys.
No leadership analytics disconnected from daily practice.
No Joey intelligence without evidence, agency, privacy, and validation.
The one-sentence repositioning
Constellation is a system-of-systems product framework that converts complex, multi-role visions
into outcome-driven journeys, vertical slices, capability maps, and BMAD-executable
implementation plans while preserving coherence across architecture, workstreams, data, and
product intent.
Or more simply:
BMAD helps AI build the product. Constellation helps AI understand the system it is building.
C153D01A-42AA-40C3-AD4A-07ACFCE04D49.jpeg
9353E7CE-902E-4A6D-BFC9-F48C3168190C.jpeg
9B48D4CA-4B81-4215-874F-E192AD2A5468.jpeg
1294AADD-1136-4C54-B8E6-F7CF1CE26D5F.jpeg
246A1DB4-3400-436B-8BEA-099FBF389416.jpeg
A1F087C5-73CA-41B4-9162-0A3C461EC8E5.jpeg
BAC65E57-87AB-4D59-B8AD-E7D538384BD2.jpeg
FB956551-3038-430B-BC46-8161AE6E0825.jpeg
8CBA8802-A332-4D27-81C5-0426EAE1CC0E.jpeg
6D4CAA0F-6A06-4B84-85A5-6B74C07C1F25.jpeg
Read these and put them into code blocks. 
Some text is small/angled, so this is the clean readable extraction from the screenshots:
Markdown
# Auspex Architecture — LENS Topology Redesign
## 1. Executive Summary
This document formalizes the architecture proposed during the Auspex brainstorming session for 
restructuring how LENS/BMAD stores, navigates, and evolves project knowledge.
The current topology works adequately for one-shot work, but it breaks down for organic, team-scale 
projects where features accumulate into services, services collaborate into domains, and domains may 
themselves participate in a larger program or product. The failure is not only structural; it is 
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
38/88
cognitive. Machines can search scattered documents. Humans need stable maps.
The proposed solution is a Two-Tree Model with Derived Map:- a Feature Archive that is permanent, flat, and never reorganized- a Landscape of service/domain/program ledgers that can be reorganized as the product topology 
evolves- a Derived Map that is generated from both trees and used by agents, tooling, tests, governance, 
and authored truth- a Salmon Workflow that handles upstream-impact discoveries through recursive validation across the 
topology
This architecture is designed to support both present-day operating reality and future maturation 
without forcing a rigid process too early.
Markdown
## 2. Problem Statement
The current artifact topology creates two distinct failure modes.
### 2.1 Failure Mode A — Knowledge Consolidation
When the design truth of a service or domain is spread across multiple feature folders and branches, 
there is no single reliable place to answer:- what does the thing currently do?- downstream work must reconstruct service truth from individual feature artifacts- higher-order views like user journeys or program flows become fragmented
### 2.2 Failure Mode B — Cross-feature Dependency at Authoring Time
A new feature depends on context from sibling or predecessor features. Under the current model, that 
context is often:- stored in another feature folder- trapped in another branch- referenced manually- dependent on contributors to rely on memory, tribal knowledge, or manual hunting
Markdown
## 3. Architecture Principles
| Principle | Meaning |
|---|---|
| P1 — Human-first consolidation | A human must have a stable place to read the current truth at 
each meaningful layer. |
| P2 — Machine-derived projection | Tooling may maintain indexes and graphs, but those are 
projections, not source truth. |
| P3 — Stable identity over mutable location | IDs are stable; paths are addresses that may change 
for landscape artifacts. |
| P4 — Promotable topology | A standalone script or feature can grow into a service, domain, or 
program without redesign. |
| P5 — Features are contributors, not durable truth | Feature artifacts record work and local 
decisions, but higher-order truth lives above them. |
| P6 — Upstream impact is first-class | A feature discovered at one feature layer must be able to 
influence service/domain/program layer. |
| P7 — Present operating model over aspirational purity | The architecture must work now, without 
depending on heavyweight PR-gated planning workflows. |
Markdown
## 4. Why the Current Branch Model Fails
Historically, the planning branch tried to solve for a more mature approval-oriented design process. 
In theory, that offers cleaner review boundaries and release discipline.
In practice, it creates disproportional value:- planning branches create documents- feature branches create code- feature visibility is reduced- the team operates in a pocket universe- planning validity becomes tied to git topology instead of document state
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
39/88
The conclusion from the brainstorming session is explicit:
Planning artifacts should no longer depend on a planning branch for legitimacy.
Instead, planning artifacts should live in stable locations and use explicit metadata such as draft 
or published status.
Markdown
## 5. Core Architecture — The Two-Tree Model
### 5.1 Tree One — Feature Archive
The architecture separates immutable feature history from reorganizable higher-order knowledge.
`docs/features/` is the permanent home of all features.
Characteristics:- append-only by default- flat or lightly organized- each feature keeps its own scratchpad, WIP notes, and closed artifacts- each feature has permanent source truth for feature identity and attachment- features are never moved once created
Example:
docs/
features/
auspex/
feature.yaml
index.md
widget-api/
feature.yaml
index.md
Markdown
### 5.2 Tree Two — Landscape
The Landscape contains the living higher-order knowledge homes.
Characteristics:- reorganizable over time- contains current service/domain/program ledgers- supports additive depth; a service can later gain a parent domain, or domain can later become part 
of a program- is where humans should go first for current truth
Example:
docs/
service/
widget-api/
service.yaml
index.md
widget-platform/
widget-api/
ledger/
enterprise-suite/
widget-platform/
widget-api/
ledger/
Markdown
### 5.3 Separation of Concern
| Area | Purpose | Audience |
|---|---|---|
| Feature Archive | Archaeological record of work, decisions, and artifacts produced during a 
feature’s life | Contributors |
| Landscape | Living present-state truth for service, domain, and program interpretation | Humans 
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
40/88
across roles |
| Derived Map | Machine-readable indexed ownership graph and signal state | Agents/tooling |
This avoids contaminating feature knowledge with current-state knowledge.
Markdown
## 6. Entity Model
The architecture recognizes four entity kinds.
| Kind | Role | Durability |
|---|---|---|
| Feature | Unit of work and local rationale | Permanent archive |
| Service | Accumulated truth across related features | Living ledger |
| Domain | Coherent cross-service capability and user-journey layer | Living ledger |
| Program / Product | Cross-domain assembly into a finished product | Living ledger |
The important nuance is that these are not mandatory levels. They are additive, introduced when the 
work demands them.
Markdown
## 7. Identity and Addressing
The redesign requires a strict separation between identity and path.- feature IDs, service IDs, domain IDs, and program IDs are stable identities- paths are addresses that can change- the Feature Archive uses effectively permanent paths- the Landscape may reorganize over time as product relationships evolve- the Derived Map preserves historical and current references
This enables growth without demanding all-at-once reorganization.
Markdown
## 8. Source of Truth and Derived Map
The machine-readable map is not authoritative.
### 8.1 Source of Truth
Source truth lives in the control-repo files themselves, primarily in frontmatter / YAML metadata:- feature.yaml- service.yaml- domain.yaml- program.yaml
### 8.2 Derived Projection
The Derived Map is generated from source files.
It may include:- built from source files- regenerated on demand- approved/merged- recoverable if lost or corrupted- intentionally treated as a projection only
### 8.3 Minimum Feature Metadata
A feature should have enough metadata to identify it, place it in the archive, and connect it to 
related landscape nodes.
YAML
feature:
id: widget-api-ledger
kind: feature
status: active
branch: feat/widget-api
belongs_to:
service: widget-api-ledger
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
41/88
YAML
feature:
id: widget-api-ledger
kind: service
belongs_to:
domain: widget-platform
Markdown
## 8.4 Minimum Service / Domain / Program Metadata
When a service exists without a domain or program, that same ledger can temporarily live at:
docs/widget-api/ledger
The service ID remains stable.
If the service later becomes part of a domain, the service can move without changing identity.
## 8.5 Projection Rebuild Behavior
Scan feature and landscape metadata
Rebuild all indexes and maps
Reconnect parent-child ownership graph
Cross-validate declarations in both directions
Create projection even from partial or inconsistent state
Rebuild governance projection target
This also enables a future `lens doctor` command for topology audit and repair guidance.
Markdown
## 9. Salmon Workflow
The brainstorming session introduced a new first-class workflow: Salmon.
### 9.1 Purpose
Sometimes feature implementation discovers a reality that should flow downstream. Sometimes feature 
implementation discovers a reality that should force reconsideration at the service, domain, or 
program level.
### 9.2 Behavior
A feature can raise an upstream-impact signal.
Examples:- the feature breaks an existing service assumption- the feature invalidates a domain journey- the feature exposes a missing capability- the feature reveals that a service boundary is wrong- the feature changes the recurring consistency check- if the check fails materially or repeatedly, the result may block progression- if the check marks the discovered impact not from the signaling, Salmon is therefore a 
consistency-maintenance workflow, not just an alert channel.
Markdown
## 10. Lens Capability Changes Required
To support this architecture, Lens must evolve in the following ways:
### 10.1 Stable IDs and Parent References
Lens must stop treating paths as identity and begin treating IDs as the canonical linkage mechanism.
IDs and parent references must be stable because they contain ledgers and references, not the other 
way around.
3BB2989D-7A80-458B-9E17-12175CC82BAB.jpeg
45D01C72-EFC9-4475-970D-651035F63F89.jpeg
A99019D3-25B3-46CF-941A-26063FABD136.jpeg
11DC389D-FEC2-48CA-8982-59816818D462.jpeg
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
42/88
Include these as well
Here are the additional readable sections:
Markdown
## 10.2 First-Class Higher-Order Entities
Lens must support `service`, `domain`, and `program` artifacts as entities with their own metadata, 
lifecycle, and ledger homes.
## 10.3 Derived Map Rebuild
Lens must add a command to rebuild the topology projection from source files.
## 10.4 Doctor / Audit
Lens should include a topology audit command capable of identifying:- orphaned features- mismatched parent-child refs- missing referenced ledgers- completed features that have not been promoted into the living landscape
## 10.5 Salmon Workflow Support
Lens must support upstream-impact signaling and recursive consistency traversal.
## 10.6 Replace Planning-Branch Assumptions
Planning artifact validity should be based on explicit document metadata rather than branch 
placement.
Markdown
## 11. Recommended Rollout Strategy
This architecture should be introduced incrementally.
### 11.1 Minimum Viable First Step
1. Extend frontmatter / YAML schema with stable IDs and `belongs_to`
2. Build projection script or command
3. Introduce `docs/features/` for all new work
4. Add one service ledger for an existing domain
5. Add a lightweight `lens-doctor`
### 11.2 Suggested Sequence
| Step | Change | Outcome |
|---|---|---|
| 1 | Extend metadata schema | Decouple identity from path |
| 2 | Build projection/rebuild command | Make the map derived and recoverable |
| 3 | Start using `docs/features/` for new work | Establish permanent feature archive pattern |
| 4 | Pilot one landscape ledger | Validate living current-state knowledge |
| 5 | Add Salmon workflow | Build trust through audit propagation |
| 6 | Expand to domain/program ledgers as needed | Support larger organic systems |
Markdown
## 12. Risks and Mitigations
| Risk | Why it matters | Mitigation |
|---|---|---|
| Dual truth drift | Feature archive and living landscape can diverge | Make promotion explicit, 
visible, and eventually agent-assisted |
| Projection drift | Derived map can become stale | Rebuild from source, never hand-edit |
| Over-modeling | Teams may create structure too early | Keep depth optional; add layers only when 
justified |
| Salmon overhead | Too many upstream signals can create noise | Default to advisory; escalate only 
when recursive checks find material impact |
| Migration fatigue | Retrofactive full migration would be disruptive | Start with new work and 
pilot areas |
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
43/88
Markdown
## 13. Strategic Recommendation
The strongest conclusion from the brainstorming session is this:
Do not choose between branch-local feature history and stable human knowledge.
Instead, separate them.
The architecture should first build the topology that matches how work actually evolves now, and 
leave room for more formal controls later.
That means implementing the enabling structure first:- stable IDs- derived projections- service/domain/program ledgers- living landscape- feature archive- recursive consistency validation- Salmon workflow
Markdown
## 14. Decision Summary
| Area | Decision |
|---|---|
| Primary topology | Two-Tree Model with Derived Map |
| Feature location | Permanent under `docs/features/` |
| Higher-order knowledge | Reorganized into landscape ledgers |
| Machine-readable topology | Derived projection, never authoritative source truth |
| Adoption strategy | Salmon workflow with recursive validation |
| Planning validity | Evolution away from big-bang migration |
Markdown
## 15. Next Design Step
This document is strong enough to move into a more detailed implementation design.
The next logical artifacts should define:- concrete file schemas for `feature/service/domain/program` metadata- projection/rebuild algorithm- command design for projection rebuild, doctor, and Salmon- migration strategy for next-work versus legacy content
Reexamine all of this based in order to outline the net new version of lens with no backwards 
compatibility needed. Integrate these images as well as ideas or concepts to take into account. 
Output them into code blocks for further analysis. Then after your analysis and integration step me 
through how as a end user of the new lens version I’d interact with it, what commands I’d use and 
an example from the Northstar page showing how it could digest such a large system into smaller 
pieces and become more and more focused as time goes on in the process and how it 
interconnects with other aspects in the process. 
The images are additional context but the main goal is designing a system that can handle 
complexity at the scale of Northstar. Northstar was just an example and one would not have such a 
website in reality detailing a complex system. They would need to use this process to help 
brainstorm and extract that design and system into the new process 
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
44/88
Exactly. The NorthStar page should be treated as a finished narrative artifact, not as the normal input.
In real life, a complex product would begin as fragments: founder vision, whiteboard sketches, scattered
stakeholder comments, existing workflows, institutional pain, partial architecture, old docs, and
ambiguous goals. The new LENS has to extract the NorthStar-level system model through a guided
process, not assume that model already exists.
So the real framework is not:
Import complete product description
→ decompose it
→ build slices
It is:
Unclear vision
→ guided discovery
→ concept extraction
→ system modeling
→ role/outcome mapping
→ journey discovery
→ vertical slicing
→ capability and topology formation
→ BMAD execution
→ validation and learning
The NorthStar page becomes an example of what LENS should be able to produce, not what it should
require.
Markdown
# Reframed Core Idea
The net-new LENS should be a guided system-discovery and decomposition framework
for large complex products.
Its purpose is not merely to organize work.
Its purpose is to help humans and AI collaboratively discover, model, refine,
decompose, build, and validate a complex product-system over time.
LENS should assume that the system is not initially known.
It should be able to start from:- a vague idea- a founder conversation- stakeholder interviews- a whiteboard- screenshots- product notes- scattered docs- partial repos- business goals- regulatory constraints- support tickets- domain expert explanations- existing workflows- aspirational outcomes
Then it should progressively produce:- system thesis- stakeholder map- role map- operating loop- outcome matrix- journey graph- concept glossary- capability map- landscape topology
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
45/88
- vertical slices- workstream graph- risk register- BMAD planning packets- implementation evidence- stakeholder status views
Markdown
# The New LENS Positioning
LENS is an AI-native system discovery, decomposition, and execution-governance
framework for large complex products.
It helps a team move from unclear product ambition to validated vertical slices
without losing the coherence of the larger system.
BMAD is the execution method.
LENS is the system-thinking layer before, around, and after BMAD.
LENS owns:- brainstorming- extraction- system modeling- topology- journey mapping- vertical slicing- workstream awareness- impact analysis- evidence tracking- upstream change propagation- stakeholder visibility
BMAD owns:- formal product brief- PRD- UX specification- architecture- epics- stories- implementation flow- code review flow
The relationship:
LENS discovers and maintains the product-system.
BMAD plans and executes build work.
LENS validates that built work still serves the intended system outcome.
The screenshots matter because they add the knowledge topology needed to make this sustainable.
For a system at NorthStar scale, the core challenge is not only “how do we brainstorm?” It is also “where
does the knowledge live after the brainstorm, how does it change safely, and how do people avoid
drowning in stale documents?”
Markdown
# Concepts from the Images Integrated into the New LENS
The screenshots contribute several core architectural ideas.
## 1. Human-First Knowledge Consolidation
Humans need stable places to read current truth.
Search is not enough.
A large system needs living ledgers that explain:- what this part of the system is- why it exists
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
46/88
- what decisions constrain it- what journeys it participates in- what capabilities it owns- what has changed recently- what risks or unresolved questions remain
## 2. Two-Tree Knowledge Model
LENS should maintain two knowledge structures:
### Work Archive
An immutable or append-only record of what happened.
This includes:- brainstorming sessions- raw notes- slice runs- stories- decisions- discarded ideas- validation results- Salmon signals- BMAD packets- implementation evidence
The Work Archive preserves history.
### Landscape
The living, current interpretation of the system.
This includes:- system ledgers- program ledgers- domain ledgers- capability ledgers- service ledgers- journey ledgers- workstream ledgers
The Landscape is where humans go to understand current truth.
## 3. Derived Map
The machine-readable graph is generated from source truth.
It is not the source of truth.
It can be rebuilt, audited, repaired, and regenerated.
The Derived Map powers:- agent context- impact analysis- dependency detection- traceability- dashboards- reporting- BMAD packet generation- validation checks
## 4. Stable Identity Over Mutable Location
IDs are permanent.
Paths can change.
A concept may start as a rough capability, later become a domain, later become
part of a larger program.
The ID survives those moves.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
47/88
## 5. Planning Artifact Validity Without Planning Branches
Planning artifacts should not be considered valid because of branch placement.
They should be valid because of metadata:- draft- reviewed- approved- blocked- superseded- archived
## 6. Salmon Workflow
Downstream work can reveal upstream truth.
An implementation slice might discover:- a role assumption is wrong- a service boundary is wrong- a journey is incomplete- a privacy policy is missing- a capability does not exist yet- an outcome is too vague- a related workstream is affected
Salmon lets that discovery swim upstream through:
story → slice → journey → outcome → capability → domain → program → system
## 7. Auspex Evidence Plane
LENS should include a read-only stakeholder visibility layer.
It should show:- system status- active journeys- active slices- risks- blockers- artifact freshness- validation evidence- open decisions- BMAD artifact links- source traceability
Stakeholders should not need repo access to understand project health.
The biggest change from the earlier framing is this:
Markdown
# Important Correction
NorthStar should not be viewed as "an imported product page."
It should be viewed as "the kind of coherent system narrative LENS helps a team
create after discovery."
In real use, the user would not begin with:
"Here is a complete website explaining the product."
They would begin with something more like:
"We want to build a platform that helps students, teachers, coaches, and leaders
coordinate learning improvement. There will probably be AI guidance, dashboards,
evidence, pathways, and some kind of professional learning loop."
LENS must then help extract:- Who are the roles?- What outcomes matter?
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
48/88
- What repeated loop does the system support?- What evidence moves through the system?- What decisions are made?- What does AI do?- What should AI not do?- Where are the trust boundaries?- What journeys matter first?- What capabilities are implied?- What should be built first?
Markdown
# Net-New LENS System
## Name
LENS
## Expanded Meaning
Large-system
Exploration,
Navigation,
Slicing,
and validation framework
## Core Promise
LENS helps teams discover, model, decompose, build, and validate complex
multi-role product systems without collapsing them into disconnected features.
## Core Flow
1. Discover the system
2. Extract concepts
3. Model roles and outcomes
4. Identify operating loops
5. Discover journeys
6. Slice journeys vertically
7. Derive capabilities
8. Build landscape ledgers
9. Analyze impact and workstreams
10. Generate BMAD planning packets
11. Guard implementation stories
12. Validate outcomes
13. Update the living landscape
14. Surface status through Auspex
YAML
# Core Entity Model
entities:
system:
description: "The whole product-system being discovered and built."
examples:- northstar-like-education-platform- enterprise-care-coordination-platform- logistics-command-platform
system_thesis:
description: "Current best explanation of what the system is and why it exists."
maturity:- sketch- hypothesis- reviewed- approved- superseded
role:
description: "A human or system actor that experiences, operates, or governs the system."
stakeholder:
description: "A person or group with influence, needs, constraints, or decision power."
outcome:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
49/88
    description: "A desired change for a role, group, business, or system."
  operating_loop:
    description: "A repeated cycle the product enables or improves."
    examples:
      - assess_set_goals_learn_apply_evidence_improve
      - detect_triage_intervene_monitor_close
      - request_approve_fulfill_audit_improve
  journey:
    description: "An end-to-end path through which an outcome becomes real."
  slice:
    description: "A thin, testable, end-to-end implementation path through a journey."
  capability:
    description: "A durable ability the system must have."
  domain:
    description: "A coherent area of product meaning, capability, or workflow."
  service:
    description: "A technical/runtime boundary derived later from capabilities and architecture."
  workstream:
    description: "A coordinated stream of work that may span slices, capabilities, and domains."
  artifact:
    description: "Any source, generated, planning, design, code, or evidence document."
  decision:
    description: "A durable choice that constrains future work."
  assumption:
    description: "A belief used for planning that may later be validated or invalidated."
  unknown:
    description: "A question that must be answered to reduce ambiguity."
  risk:
    description: "A condition that can damage product coherence, safety, delivery, trust, or 
usability."
  evidence:
    description: "Proof that a slice, journey, or outcome has been satisfied."
  salmon_signal:
    description: "A downstream discovery that may need to alter upstream system truth."
YAML
# Knowledge State Model
knowledge_states:
  raw:
    meaning: "Captured directly from a user, session, whiteboard, document, or source."
  extracted:
    meaning: "Identified as a possible concept, role, outcome, journey, risk, or capability."
  hypothesized:
    meaning: "Structured by LENS but not yet confirmed."
  challenged:
    meaning: "Reviewed for contradictions, missing pieces, or weak assumptions."
  reviewed:
    meaning: "Human reviewed and accepted as plausible."
  approved:
    meaning: "Accepted as current working truth."
  superseded:
    meaning: "Replaced by a newer model."
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 50/88
  archived:
    meaning: "Kept for history but no longer active."
confidence_levels:
  low:
    meaning: "Weak signal, inferred, or ambiguous."
  medium:
    meaning: "Likely correct but not fully validated."
  high:
    meaning: "Confirmed by stakeholder, artifact, implementation, or repeated evidence."
source_types:
  - brainstorm_session
  - stakeholder_interview
  - whiteboard
  - uploaded_image
  - product_note
  - existing_document
  - codebase_scan
  - support_ticket
  - operational_workflow
  - regulatory_constraint
  - BMAD_artifact
  - implementation_result
  - validation_result
This provenance model is critical. Without it, AI will overstate guesses as facts. A large-system
framework needs to know whether something is known, inferred, contested, or still just a useful
hypothesis.
YAML
# Example Concept Provenance
concept:
  id: outcome.teacher-turns-evidence-into-action
  type: outcome
  status: hypothesized
  confidence: medium
  statement: >
    Teachers need to convert student evidence into a next instructional action.
  provenance:
    - source: session.discovery.001
      source_type: brainstorm_session
      quote_or_summary: >
        User described teachers needing practical guidance from student work.
      confidence: medium
    - source: interview.teacher.003
      source_type: stakeholder_interview
      quote_or_summary: >
        Teacher described evidence review as time-consuming and disconnected
        from next-step planning.
      confidence: high
  open_questions:
    - Should guidance be generated automatically or requested by the teacher?
    - Can coaches see teacher decisions?
    - What student-level data can be surfaced to leadership?
  related:
    roles:
      - role.teacher
      - role.student
      - role.coach
    capabilities:
      - capability.evidence_capture
      - capability.guidance_generation
      - capability.teacher_decision_capture
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 51/88
The real user journey: from vague idea to buildable system
The user should not need to know the final system structure up front. LENS should act like a workshop
facilitator, system cartographer, and build orchestrator.
User has vague idea
↓
LENS runs discovery session
↓
LENS extracts candidate concepts
↓
LENS builds a rough system thesis
↓
LENS challenges assumptions and gaps
↓
LENS conducts role/outcome discovery
↓
LENS identifies operating loops
↓
LENS maps journeys
↓
LENS slices journeys
↓
LENS derives capabilities
↓
LENS forms living landscape ledgers
↓
LENS prepares BMAD packet
↓
BMAD creates formal planning/build artifacts
↓
LENS guards implementation
↓
LENS validates slice/journey/outcome
↓
LENS updates the system landscape
Bash
# 1. Start with no finished product description
lens new system northstar-like-platform --blank
LENS response:
Created blank system workspace.
Current known state:- system name: northstar-like-platform- system thesis: unknown- roles: unknown- outcomes: unknown- journeys: unknown- capabilities: unknown- architecture: unknown
Recommended next step:
lens discover start --mode brainstorm
Bash
# 2. Begin guided discovery
lens discover start \--system northstar-like-platform \--mode brainstorm \--goal "Explore an education platform for students, teachers, coaches, and leaders"
Markdown
# Discovery Session Prompts
LENS should guide the user through structured exploration.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
52/88
## System Intent- What problem are you trying to solve?- Who experiences the pain today?- What changes if the system succeeds?- What would be impossible or much easier after this system exists?- What must the system never do?
## Roles- Who uses the system daily?- Who benefits from the system indirectly?- Who governs or supervises the system?- Who supplies data or evidence?- Who acts on recommendations?- Who needs visibility but should not micromanage?
## Workflows- What repeated process does this product improve?- Is there a loop?- Is there a before/during/after cycle?- What triggers the workflow?- What is the end state?
## Evidence- What artifacts, data, or signals move through the system?- What does the system need to remember?- What should be auditable?- What should be visible to whom?
## Intelligence / AI- Where should AI help?- Where should AI not decide?- What should remain human-controlled?- What explanations are required?- What failure modes are unacceptable?
## Scale- Is this for one user, one team, one school, one district, many organizations?- What needs to work locally first?- What should roll up later?- What should stay decentralized?
## Risks- What would make the system dangerous, untrusted, or unusable?- What privacy boundaries exist?- What decisions require human approval?- What assumptions are weakest?
Bash
# 3. Synthesize the first system thesis
lens discover synthesize --system northstar-like-platform
YAML
# Generated: .lens/landscape/systems/northstar-like-platform/thesis.yaml
system:
id: system.northstar_like_platform
name: NorthStar-like Platform
status: hypothesis
confidence: low
thesis: >
A multi-role education improvement platform that helps students, teachers,
coaches, and leaders coordinate learning goals, evidence, guidance,
professional support, and system-level improvement.
likely_roles:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
53/88
    - id: role.student
      confidence: high
    - id: role.teacher
      confidence: high
    - id: role.coach
      confidence: medium
    - id: role.school_leader
      confidence: medium
    - id: role.district_leader
      confidence: medium
    - id: role.family
      confidence: low
  candidate_operating_loop:
    id: loop.learning_improvement
    status: hypothesis
    confidence: medium
    steps:
      - assess
      - set_goals
      - learn
      - apply
      - collect_evidence
      - improve
  candidate_intelligence_layer:
    id: intelligence.guidance_assistant
    status: hypothesis
    confidence: medium
    responsibilities:
      - analyze_learning_evidence
      - identify_strengths_and_needs
      - suggest_next_actions
      - explain_recommendations
      - surface_patterns
  open_questions:
    - Are families a primary user or secondary stakeholder?
    - Does the system own assessments or consume assessment data?
    - Are recommendations advisory or directive?
    - What evidence is visible to coaches?
    - What data can leadership see?
Bash
# 4. Ask LENS to challenge the model before going deeper
lens challenge system northstar-like-platform
Markdown
# Challenge Report
## Possible Contradictions
1. The system wants leadership visibility but also needs to avoid micromanagement.
   This requires a visibility policy.
2. The system wants AI-generated guidance but also needs teacher agency.
   This requires an advisory model and override path.
3. The system wants student evidence to inform coaching.
   This may create privacy and role-boundary concerns.
4. The system may serve students, teachers, coaches, leaders, and families.
   This is too broad for a first implementation slice.
## Missing Decisions- What is the first role to serve?- What is the first outcome to prove?- What is the minimum evidence object?- What is the first operating loop?- What does AI do in MVP?- What should remain manual at first?
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 54/88
## Recommended Next Step
Run role/outcome discovery before architecture.
Suggested command:
lens discover outcomes --system northstar-like-platform
Bash
# 5. Discover role outcomes
lens discover outcomes --system northstar-like-platform
YAML
# Generated: .lens/intent/outcomes/role-outcome-matrix.yaml
system: system.northstar_like_platform
status: hypothesis
role_outcomes:- role: student
candidate_outcomes:- id: outcome.student_understands_progress
statement: Student understands current progress and next goals.
confidence: medium- id: outcome.student_follows_pathway
statement: Student follows a personalized learning pathway.
confidence: low- role: teacher
candidate_outcomes:- id: outcome.teacher_turns_evidence_into_action
statement: Teacher converts student evidence into a next instructional action.
confidence: high- id: outcome.teacher_monitors_growth
statement: Teacher monitors growth across students and goals.
confidence: medium- role: coach
candidate_outcomes:- id: outcome.coach_identifies_support_needs
statement: Coach identifies teacher support needs from implementation evidence.
confidence: medium- role: school_leader
candidate_outcomes:- id: outcome.leader_sees_implementation_health
statement: Leader sees implementation health without micromanaging.
confidence: medium- role: district_leader
candidate_outcomes:- id: outcome.district_allocates_support
statement: District leader identifies where support is needed across schools.
confidence: low
Bash
# 6. Select a focus outcome
lens focus outcome outcome.teacher_turns_evidence_into_action
LENS response:
Focused outcome:
Teacher converts student evidence into a next instructional action.
Why this is a strong first focus:- clear primary user- clear evidence object- connects student learning to adult action- exposes important AI, privacy, and dashboard questions early
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
55/88
- can later connect to coaching and leadership rollups
Recommended next step:
lens discover journey --outcome outcome.teacher_turns_evidence_into_action
Bash
# 7. Discover the journey
lens discover journey \
  --outcome outcome.teacher_turns_evidence_into_action
YAML
# Generated: .lens/journeys/evidence-to-teacher-action/journey.yaml
journey:
  id: journey.evidence_to_teacher_action
  status: hypothesis
  confidence: medium
  outcome: outcome.teacher_turns_evidence_into_action
  primary_actor: role.teacher
  supporting_actors:
    - role.student
    - role.guidance_assistant
    - role.coach
  start_state:
    - student work or assessment evidence exists
    - teacher needs to understand what the evidence means
  end_state:
    - teacher chooses a next instructional action
    - system records the decision
    - downstream visibility rules are respected
  candidate_steps:
    - id: step.capture_evidence
      actor: student_or_system
      description: Evidence is created, submitted, imported, or captured.
    - id: step.store_evidence
      actor: system
      description: Evidence is stored with source metadata.
    - id: step.interpret_evidence
      actor: guidance_assistant
      description: Evidence is summarized and interpreted.
    - id: step.align_to_goal_or_standard
      actor: guidance_assistant
      description: Evidence is connected to a goal, standard, skill, or competency.
    - id: step.generate_teacher_guidance
      actor: guidance_assistant
      description: Teacher receives suggested instructional next action.
    - id: step.teacher_decides
      actor: teacher
      description: Teacher accepts, adjusts, or rejects the guidance.
    - id: step.record_decision
      actor: system
      description: Decision and rationale are recorded.
    - id: step.signal_support_need
      actor: system
      description: Appropriate coaching or support signal is created.
Bash
# 8. Slice the journey
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 56/88
lens slice journey journey.evidence_to_teacher_action
YAML
# Generated: .lens/slices/roadmap.yaml
journey: journey.evidence_to_teacher_action
slice_candidates:
  - id: slice.evidence_visible_to_teacher
    order: 1
    objective: >
      Teacher can see a student evidence artifact with source metadata.
    proves:
      - evidence_capture
      - teacher_visibility
      - basic_role_access
  - id: slice.evidence_interpreted_for_teacher
    order: 2
    objective: >
      Teacher can see a plain-language summary of what the evidence suggests.
    proves:
      - evidence_interpretation
      - teacher_dashboard_context
      - explanation_baseline
  - id: slice.evidence_aligned_to_goal
    order: 3
    objective: >
      Evidence can be aligned to a goal, standard, skill, or competency.
    proves:
      - goal_or_standard_model
      - alignment_logic
      - evidence_meaning
  - id: slice.guidance_recommended_to_teacher
    order: 4
    objective: >
      Teacher receives an explainable recommended next action.
    proves:
      - guidance_generation
      - teacher_agency
      - recommendation_rationale
  - id: slice.teacher_decision_recorded
    order: 5
    objective: >
      Teacher accepts, adjusts, or rejects the recommendation and the decision is recorded.
    proves:
      - decision_capture
      - auditability
      - human-in-the-loop control
  - id: slice.coach_signal_created
    order: 6
    objective: >
      A coaching signal is created from teacher action without exposing inappropriate data.
    proves:
      - coaching_visibility_policy
      - role-boundary protection
      - downstream support loop
Bash
# 9. Focus on the first practical slice
lens focus slice slice.evidence_visible_to_teacher
YAML
# Generated: .lens/slices/evidence-visible-to-teacher/slice.yaml
slice:
  id: slice.evidence_visible_to_teacher
  status: selected
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 57/88
  maturity: planning
  objective: >
    Teacher can see a student evidence artifact with source metadata.
  journey: journey.evidence_to_teacher_action
  outcome: outcome.teacher_turns_evidence_into_action
  why_first: >
    This establishes the minimum evidence object, visibility rule, and teacher
    workspace touchpoint before introducing AI guidance or coaching rollups.
  starts_with:
    - student evidence artifact exists
    - teacher has relationship to student or class
    - artifact has source metadata
  ends_with:
    - teacher can view artifact
    - teacher can see source and timestamp
    - unauthorized users cannot view artifact
    - missing artifact state is handled clearly
  vertical_path:
    experience:
      - teacher evidence inbox
      - evidence detail page
    data:
      - EvidenceArtifact
      - EvidenceSource
      - StudentContext
      - TeacherAccessPolicy
    policy:
      - teacher can view evidence for assigned students
      - source metadata must be visible
      - stale or missing evidence must be indicated
    services_or_components:
      status: not_final
      candidates:
        - evidence_store
        - roster_context
        - teacher_workspace
  acceptance_evidence:
    - teacher can open evidence artifact
    - artifact source is visible
    - artifact timestamp is visible
    - unauthorized teacher cannot access artifact
    - missing artifact renders safe empty state
This is the moment where the system becomes buildable. Notice that the first slice does not try to build
the whole NorthStar-like system. It proves a narrow vertical path that future slices can build on.
Bash
# 10. Derive capabilities from the focused slice
lens map capabilities --slice slice.evidence_visible_to_teacher
YAML
# Generated: .lens/slices/evidence-visible-to-teacher/capability-map.yaml
required_capabilities:
  - id: capability.evidence_artifact_model
    status: hypothesis
    reason: The system needs a durable representation of evidence.
  - id: capability.evidence_source_metadata
    status: hypothesis
    reason: Teachers need to know where evidence came from and when it was captured.
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 58/88
  - id: capability.teacher_student_relationship
    status: hypothesis
    reason: Access depends on teacher/student/class association.
  - id: capability.role_based_visibility
    status: hypothesis
    reason: Evidence visibility must be controlled by role and relationship.
  - id: capability.teacher_workspace
    status: hypothesis
    reason: Teacher needs a daily-work surface where evidence appears.
  - id: capability.stale_or_missing_source_handling
    status: hypothesis
    reason: The UI must show when evidence is missing, stale, or failed to load.
Bash
# 11. Promote capabilities into the living landscape
lens promote capabilities --from-slice slice.evidence_visible_to_teacher
YAML
# Generated: .lens/landscape/capabilities/evidence-artifact-model/capability.yaml
capability:
  id: capability.evidence_artifact_model
  name: Evidence Artifact Model
  status: active
  maturity: hypothesis
  purpose: >
    Represent student evidence in a durable, source-traceable way.
  discovered_from:
    - slice.evidence_visible_to_teacher
  participates_in:
    journeys:
      - journey.evidence_to_teacher_action
    outcomes:
      - outcome.teacher_turns_evidence_into_action
  open_questions:
    - What counts as evidence?
    - Can evidence be imported from external systems?
    - Can evidence be manually created?
    - Does evidence belong to a student, class, goal, standard, or all of these?
  related_capabilities:
    - capability.evidence_source_metadata
    - capability.role_based_visibility
Bash
# 12. Run impact and workstream analysis
lens analyze impact --slice slice.evidence_visible_to_teacher
YAML
# Generated: .lens/slices/evidence-visible-to-teacher/impact-map.yaml
impact:
  slice: slice.evidence_visible_to_teacher
  impacted_roles:
    - role.teacher
    - role.student
  impacted_capabilities:
    - capability.evidence_artifact_model
    - capability.evidence_source_metadata
    - capability.teacher_student_relationship
    - capability.role_based_visibility
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 59/88
- capability.teacher_workspace
candidate_domains:- domain.evidence_and_portfolios- domain.teacher_workspace- domain.identity_and_access
candidate_services:
status: deferred_until_architecture
candidates:- service.evidence_service- service.roster_service- service.teacher_dashboard_bff
risks:- id: risk.privacy_boundary
description: Teachers must only see evidence for students they are allowed to support.- id: risk.evidence_definition_ambiguity
description: The product has not yet defined what qualifies as evidence.- id: risk.source_staleness
description: Evidence may be stale, imported late, or fail to load.
required_decisions:- id: decision.minimum_evidence_shape
question: What fields must every evidence artifact have?- id: decision.teacher_access_policy
question: What relationship grants a teacher access to a student artifact?- id: decision.source_freshness_display
question: How does the UI show stale or missing evidence?
Bash
# 13. Prepare BMAD packet only after the slice is coherent
lens prepare bmad --slice slice.evidence_visible_to_teacher
Markdown
# Generated BMAD Packet Summary
## System
NorthStar-like education improvement platform.
## Focus Outcome
Teacher converts student evidence into a next instructional action.
## Focus Journey
Evidence to Teacher Action.
## Focus Slice
Evidence Visible to Teacher.
## Slice Goal
Teacher can see a student evidence artifact with source metadata.
## Why This Slice Comes First
This establishes the minimum evidence model, role-based visibility, and teacher
workspace surface before introducing AI interpretation, goal alignment, or
coaching rollups.
## Required Capabilities- Evidence Artifact Model- Evidence Source Metadata- Teacher/Student Relationship- Role-Based Visibility
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
60/88
- Teacher Workspace- Missing/Stale Evidence Handling
## Required Decisions Before Implementation- Minimum evidence shape- Teacher access policy- Source freshness display- Missing artifact behavior
## Acceptance Evidence- Teacher can view permitted evidence.- Teacher cannot view unauthorized evidence.- Evidence source is visible.- Evidence timestamp is visible.- Missing evidence has safe empty state.- Stale evidence is visibly marked.
Then BMAD can do what BMAD is good at: formalize PRD, UX, architecture, epics, and stories. LENS
should not dump the whole messy brainstorm into BMAD. LENS should hand BMAD a focused slice
packet.
Bash
# 14. Run BMAD using the focused packet
bmad-product-brief
bmad-create-prd
bmad-create-ux-design
bmad-create-architecture
bmad-create-epics-and-stories
bmad-check-implementation-readiness
Bash
# 15. Sync BMAD outputs back into LENS
lens sync bmad --slice slice.evidence_visible_to_teacher
lens map rebuild
lens doctor
YAML
# Updated Traceability
traceability:
system: system.northstar_like_platform
outcome: outcome.teacher_turns_evidence_into_action
journey: journey.evidence_to_teacher_action
slice: slice.evidence_visible_to_teacher
bmad_artifacts:
product_brief: artifact.bmad.product_brief.evidence_visible_to_teacher
prd: artifact.bmad.prd.evidence_visible_to_teacher
ux: artifact.bmad.ux.evidence_visible_to_teacher
architecture: artifact.bmad.architecture.evidence_visible_to_teacher
epics: artifact.bmad.epics.evidence_visible_to_teacher
landscape_updates:
capabilities:- capability.evidence_artifact_model- capability.evidence_source_metadata- capability.role_based_visibility
domains:- domain.evidence_and_portfolios- domain.teacher_workspace- domain.identity_and_access
How the system becomes more focused over time
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
61/88
Very broad idea:
"Education improvement platform with AI, students, teachers, coaches, and leaders."
↓ LENS discovery
System thesis:
"Multi-role learning improvement platform."
↓ LENS role/outcome mapping
Focused outcome:
"Teacher converts student evidence into next instructional action."
↓ LENS journey discovery
Journey:
"Evidence to Teacher Action."
↓ LENS slicing
First slice:
"Evidence Visible to Teacher."
↓ LENS capability mapping
Capabilities:- evidence artifact model- source metadata- teacher/student relationship- role-based visibility- teacher workspace
↓ LENS impact analysis
Risks:- privacy boundary- stale evidence- evidence definition ambiguity
↓ LENS BMAD packet
Formal planning starts.
↓ BMAD
PRD, UX, architecture, epics, stories.
↓ LENS validation
Did the slice prove the journey step?
↓ LENS landscape update
The system model becomes more accurate.
What makes this different from normal product planning
Normal planning tries to define the whole product too early.
The new LENS should allow the system to emerge through structured discovery and increasingly
focused slices.
Markdown
# LENS Design Principle
Do not ask humans to define the entire system up front.
Instead:
1. capture raw thinking
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
62/88
2. extract candidate concepts
3. mark confidence
4. challenge assumptions
5. select a focus outcome
6. map one journey
7. slice one vertical path
8. derive capabilities
9. build a little
10. validate
11. update the living landscape
12. repeat
Large systems are not specified once.
They are progressively discovered, built, validated, and reorganized.
The command model should support conversational discovery
The commands should not only be technical. They should support workshops.
lens new system
lens discover start
lens discover synthesize
lens discover roles
lens discover outcomes
lens discover loop
lens discover journeys
lens challenge system
lens focus outcome
lens focus journey
lens slice journey
lens map capabilities
lens promote capabilities
lens analyze impact
lens prepare bmad
lens sync bmad
lens guard story
lens salmon raise
lens validate slice
lens validate journey
lens validate outcome
lens auspex build
lens auspex serve
Bash
# Example full flow without a product page
lens new system learning-improvement-platform --blank
lens discover start \--mode brainstorm \--goal "Explore a multi-role education improvement platform"
lens discover synthesize
lens challenge system learning-improvement-platform
lens discover roles
lens discover outcomes
lens discover loop
lens focus outcome outcome.teacher_turns_evidence_into_action
lens discover journey --outcome outcome.teacher_turns_evidence_into_action
lens slice journey journey.evidence_to_teacher_action
lens focus slice slice.evidence_visible_to_teacher
lens map capabilities --slice slice.evidence_visible_to_teacher
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
63/88
lens analyze impact --slice slice.evidence_visible_to_teacher
lens prepare bmad --slice slice.evidence_visible_to_teacher
bmad-product-brief
bmad-create-prd
bmad-create-ux-design
bmad-create-architecture
bmad-create-epics-and-stories
lens sync bmad --slice slice.evidence_visible_to_teacher
lens guard story story.create-evidence-detail-view
lens validate slice slice.evidence_visible_to_teacher
lens auspex build --system learning-improvement-platform
The “NorthStar page” becomes a generated artifact
This is the key correction.
A polished NorthStar-style page would be an output of LENS, not an input requirement.
Bash
lens publish narrative \--system learning-improvement-platform \--audience stakeholders \--format product-page
Markdown
# Generated Product Narrative
## What This System Is
A multi-role learning improvement platform that helps students, teachers,
coaches, and leaders coordinate evidence, goals, guidance, and support.
## Who It Serves- Students- Teachers- Coaches- School leaders- District leaders- Families, if confirmed as active participants
## Core Operating Loop
Assess → Set Goals → Learn → Apply → Evidence → Improve
## First Proven Journey
Evidence to Teacher Action
## First Build Slice
Evidence Visible to Teacher
## What Is Still Hypothesis- Family role- Coaching visibility policy- Leadership aggregation model- AI recommendation boundaries- Standards alignment confidence model
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
64/88
That is the right mental model: LENS helps the team produce the coherent system story through
discovery, not merely consume one.
Final integrated definition
Markdown
# Net-New LENS Definition
LENS is a guided system-discovery and decomposition framework for large,
complex, multi-role products.
It starts when the product is still ambiguous.
It helps teams brainstorm, extract, challenge, model, and progressively focus
the system into role outcomes, journeys, vertical slices, capabilities,
workstreams, BMAD-ready plans, and validation evidence.
It maintains two durable knowledge structures:
1. Work Archive- what happened- what was discussed- what was decided- what was built- what was validated
2. Living Landscape- what the system currently means- what capabilities exist- what journeys are active- what domains are forming- what services support them- what assumptions remain open
It generates machine projections for agents and dashboards.
It uses Salmon to propagate downstream discoveries upstream.
It uses Auspex to make status, evidence, freshness, and risk visible to
stakeholders.
It uses BMAD for formal planning and implementation.
The purpose is not to manage features.
The purpose is to help a team safely discover and build a complex system
without losing the coherence of the whole.
Based on this explain the layers and how they’d interact with bmad based on https://docs.bmad
method.org//llms-full.txt what they’d produce where they’d store that and how relationships etc 
would be discovered and worked through. Because lens is not standalone. It is a module of bmad. 
Below is the clean model: LENS is not a standalone product framework. It is a BMAD module that
adds a system-discovery / complexity-management layer around BMAD’s normal workflows.
BMAD already provides the core lifecycle: Analysis → Planning → Solutioning → Implementation,
with optional brainstorming/research/product brief/PRFAQ, then PRD, architecture, epics/stories, sprint
planning, story creation, implementation, and code review. BMAD Method LENS should plug into that flow as
the module that helps BMAD handle very large, ambiguous systems before they are clear enough to
become a normal PRD/backlog.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
65/88
BMAD = execution framework
LENS = complexity-discovery and system-coherence module inside BMAD
BMAD asks:
What workflow should run next?
LENS adds:
What system are we discovering?
What roles exist?
What outcomes matter?
What journeys prove those outcomes?
What vertical slices should be built first?
What capabilities and workstreams are emerging?
What relationships and impacts must BMAD preserve?
1. BMAD module placement
BMAD v6 installs modules into _bmad/ and outputs artifacts into _bmad-output/. The docs show _bmad/ as
the single installation folder and _bmad-output/ as the output folder, and custom modules can be
installed alongside official modules. BMAD Method+1
So LENS should live like this:
your-project/
├── _bmad/
│   ├── core/
│   ├── bmm/
│   ├── lens/
│   │   ├── module.yaml
│   │   ├── module-help.csv
│   │   ├── skills/
│   │   │   ├── lens-help/
│   │   │   ├── lens-discover/
│   │   │   ├── lens-synthesize/
│   │   │   ├── lens-map-system/
│   │   │   ├── lens-map-outcomes/
│   │   │   ├── lens-map-journeys/
│   │   │   ├── lens-slice-journey/
│   │   │   ├── lens-map-capabilities/
│   │   │   ├── lens-analyze-impact/
│   │   │   ├── lens-prepare-bmad/
│   │   │   ├── lens-sync-bmad/
│   │   │   ├── lens-guard-story/
│   │   │   ├── lens-salmon/
│   │   │   ├── lens-doctor/
│   │   │   └── lens-auspex/
│   │   ├── agents/
│   │   │   ├── system-cartographer/
│   │   │   ├── outcome-strategist/
│   │   │   ├── journey-architect/
│   │   │   ├── slice-designer/
│   │   │   ├── capability-mapper/
│   │   │   ├── impact-analyst/
│   │   │   ├── bmad-liaison/
│   │   │   └── outcome-validator/
│   │   └── templates/
│   └── _config/
│       └── manifest.yaml
│
└── _bmad-output/
├── lens/
├── planning-artifacts/
├── implementation-artifacts/
└── project-context.md
BMAD custom modules can be installed from local paths or Git URLs with --custom-source, and custom
modules appear in _bmad/ alongside official modules. BMAD Method
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
66/88
Bash
npx bmad-method install \--modules bmm \--custom-source ./lens-module \--tools claude-code \--yes
2. The layered architecture
LENS should have layers. Each layer produces artifacts that either stay in LENS space or feed BMAD.
Layer 0: BMAD Core Runtime
Layer 1: LENS Capture Layer
Layer 2: LENS Extraction Layer
Layer 3: LENS Intent Layer
Layer 4: LENS Journey Layer
Layer 5: LENS Slice Layer
Layer 6: LENS Capability / Landscape Layer
Layer 7: LENS Derived Map Layer
Layer 8: LENS BMAD Bridge Layer
Layer 9: LENS Implementation Guard Layer
Layer 10: LENS Salmon / Correction Layer
Layer 11: LENS Auspex Visibility Layer
The relationship is:
LENS discovers and structures complexity.
BMAD formalizes and executes software delivery.
LENS watches whether execution still matches the system intent.
3. Layer 0 — BMAD Core Runtime
This is the normal BMAD installation and workflow system.
BMAD provides:
_bmad/
_bmad-output/
bmad-help
bmad-brainstorming
bmad-product-brief
bmad-create-prd
bmad-create-ux-design
bmad-create-architecture
bmad-create-epics-and-stories
bmad-check-implementation-readiness
bmad-sprint-planning
bmad-create-story
bmad-dev-story
bmad-code-review
bmad-correct-course
BMAD’s own docs say the main phases are Analysis, Planning, Solutioning, and Implementation.
BMAD Method The docs also describe the build cycle as bmad-create-story, bmad-dev-story, and bmad-code
review. BMAD Method
BMAD owns:- formal workflow execution- PRD generation- architecture generation- epic/story generation- sprint planning- story implementation- code review- correction workflow
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
67/88
LENS should not replace those.
It should make BMAD smarter about large systems.
4. Layer 1 — Capture Layer
This layer captures messy human input.
Real complex projects do not start as clean PRDs. They start as fragments.
Inputs:- brainstorming conversations- stakeholder interviews- founder notes- whiteboard photos- partial docs- old diagrams- product screenshots- codebase hints- operational workflows- domain constraints- regulatory constraints- support tickets- existing BMAD artifacts
Storage:
_bmad-output/lens/archive/capture/
├── sessions/
│   ├── 2026-05-13-discovery-session.md
│   ├── 2026-05-13-whiteboard-notes.md
│   └── 2026-05-13-stakeholder-interview.md
├── uploads/
│   ├── whiteboard-001.jpg
│   └── sketch-002.png
└── sources.yaml
Example command:
Bash
lens-discover start \--system northstar-like-platform \--mode brainstorm
Output:
YAML
# _bmad-output/lens/archive/capture/sessions/session-001.yaml
session:
id: session.001
type: brainstorm
system: system.northstar_like_platform
status: captured
raw_inputs:- type: conversation
path: session-001-transcript.md- type: uploaded_image
path: uploads/whiteboard-001.jpg- type: stakeholder_note
path: stakeholder-notes-001.md
This layer is intentionally messy. It does not try to be the truth yet.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
68/88
5. Layer 2 — Extraction Layer
This layer reads raw material and extracts candidate concepts.
It should not immediately create final architecture.
It should extract:
candidate roles
candidate users
candidate stakeholders
candidate outcomes
candidate pain points
candidate workflows
candidate operating loops
candidate risks
candidate assumptions
candidate capabilities
candidate services
candidate constraints
candidate open questions
Storage:
_bmad-output/lens/archive/extractions/
├── extraction-001.yaml
├── extraction-002.yaml
└── extraction-index.yaml
Example command:
Bash
lens-synthesize --from session.001
Output:
YAML
# _bmad-output/lens/archive/extractions/extraction-001.yaml
extraction:
id: extraction.001
source: session.001
status: candidate
confidence: mixed
candidate_roles:- id: role.student
label: Student
confidence: high
source_refs:- session.001#p12- id: role.teacher
label: Teacher
confidence: high
source_refs:- session.001#p18- id: role.coach
label: Coach
confidence: medium
source_refs:- session.001#p27
candidate_outcomes:- id: outcome.teacher_turns_evidence_into_action
statement: Teacher can convert student evidence into a next instructional action.
confidence: medium
source_refs:- session.001#p33
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
69/88
candidate_operating_loops:- id: loop.learning_improvement
steps:- assess- set_goals- learn- apply- collect_evidence- improve
confidence: low
The important thing is provenance. LENS must know whether something came from a human, an AI
inference, a document, or a validation result.
6. Layer 3 — Intent Layer
This is where LENS starts forming the current system model.
It produces:
system thesis
role map
stakeholder map
outcome matrix
operating loop hypotheses
principles
non-goals
constraints
open questions
risk register
Storage:
_bmad-output/lens/intent/
├── system-thesis.md
├── system-thesis.yaml
├── role-map.yaml
├── stakeholder-map.yaml
├── outcome-matrix.yaml
├── operating-loops.yaml
├── principles.md
├── assumptions.yaml
├── open-questions.yaml
└── risks.yaml
Example command:
Bash
lens-map-system --system northstar-like-platform
lens-map-outcomes --system northstar-like-platform
Output:
YAML
# _bmad-output/lens/intent/system-thesis.yaml
system:
id: system.northstar_like_platform
name: NorthStar-like Platform
status: hypothesis
maturity: early-discovery
thesis: >
A multi-role education improvement platform that helps students, teachers,
coaches, and leaders coordinate learning goals, evidence, guidance, support,
and system-level improvement.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
70/88
primary_problem: >
Learning improvement work is fragmented across roles, artifacts, tools,
and decision layers.
candidate_roles:- role.student- role.teacher- role.coach- role.school_leader- role.district_leader- role.family
candidate_operating_loop:
id: loop.learning_improvement
steps:- assess- set_goals- learn- apply- evidence- improve
confidence:
thesis: medium
role_map: medium
operating_loop: low
open_questions:- Are families primary users or secondary stakeholders?- Does the system administer assessment or consume assessment data?- What decisions can AI recommend but not make?
How this interacts with BMAD:
LENS Intent Layer
→ provides structured input to:
bmad-brainstorming
bmad-product-brief
bmad-prfaq
bmad-create-prd
BMAD’s Analysis phase already includes brainstorming, research, product brief, and PRFAQ, and product
brief is recommended when the concept is clear. BMAD Method LENS gives those workflows stronger
structured context.
7. Layer 4 — Journey Layer
This layer turns outcomes into end-to-end paths.
This is critical for complex systems because features are too small and domains are too vague.
A journey answers:
Who starts where?
What are they trying to accomplish?
What other roles or systems are involved?
What evidence or data moves?
What decisions are made?
What state changes?
What does success look like?
Storage:
_bmad-output/lens/journeys/
├── journey-catalog.yaml
├── evidence-to-teacher-action/
│   ├── journey.yaml
│   ├── journey.md
│   ├── journey-map.mmd
│
└
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
71/88
│   └── open-questions.yaml
└── student-assessment-to-goal/
├── journey.yaml
├── journey.md
└── journey-map.mmd
Example command:
Bash
lens-map-journeys --outcome outcome.teacher_turns_evidence_into_action
Output:
YAML
# _bmad-output/lens/journeys/evidence-to-teacher-action/journey.yaml
journey:
id: journey.evidence_to_teacher_action
outcome: outcome.teacher_turns_evidence_into_action
status: hypothesis
primary_actor: role.teacher
supporting_actors:- role.student- role.coach- system.guidance_assistant
start_state:- student evidence exists- teacher needs to understand what to do next
end_state:- teacher selects a next instructional action- decision is recorded- downstream visibility rules are respected
steps:- id: step.capture_evidence
description: Evidence is created, submitted, imported, or captured.
actor: role.student- id: step.store_evidence
description: Evidence is stored with source metadata.
actor: system.platform- id: step.review_evidence
description: Teacher reviews the evidence artifact.
actor: role.teacher- id: step.interpret_evidence
description: Guidance assistant summarizes what the evidence suggests.
actor: system.guidance_assistant- id: step.teacher_decides
description: Teacher accepts, adjusts, or rejects a suggested action.
actor: role.teacher- id: step.record_decision
description: The decision and rationale are recorded.
actor: system.platform
How this interacts with BMAD:
Journey Layer
→ informs PRD user flows
→ informs UX workflow
→ informs architecture boundaries
→ informs epic/story grouping
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
72/88
BMAD’s Method track creates PRD, Architecture, and UX for products, platforms, and complex features.
BMAD Method LENS Journey artifacts become the structured input to those BMAD artifacts.
8. Layer 5 — Slice Layer
This is the most important LENS layer.
A slice is:
A thin, testable, end-to-end path through a journey that proves part of an outcome.
This is where a huge system becomes buildable.
Storage:
_bmad-output/lens/slices/
├── slice-roadmap.yaml
├── evidence-visible-to-teacher/
│   ├── slice.yaml
│   ├── slice.md
│   ├── acceptance-evidence.yaml
│   ├── risks.yaml
│   └── bmad-packet.md
└── evidence-to-guidance/
├── slice.yaml
├── slice.md
└── acceptance-evidence.yaml
Example command:
Bash
lens-slice-journey journey.evidence_to_teacher_action
Output:
YAML
# _bmad-output/lens/slices/slice-roadmap.yaml
journey: journey.evidence_to_teacher_action
slices:- id: slice.evidence_visible_to_teacher
order: 1
purpose: >
Teacher can view a student evidence artifact with source metadata.
proves:- basic evidence model- teacher visibility- role-based access- source metadata- id: slice.evidence_interpreted_for_teacher
order: 2
purpose: >
Teacher can see a plain-language interpretation of the evidence.
proves:- evidence interpretation- explanation baseline- teacher workspace context- id: slice.evidence_aligned_to_goal
order: 3
purpose: >
Evidence can be aligned to a goal, standard, skill, or competency.
proves:- standards or goal model- alignment logic
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
73/88
      - evidence meaning
  - id: slice.guidance_recommended_to_teacher
    order: 4
    purpose: >
      Teacher receives an explainable next-action recommendation.
    proves:
      - guidance generation
      - teacher agency
      - human-in-the-loop decision
Selected slice:
YAML
# _bmad-output/lens/slices/evidence-visible-to-teacher/slice.yaml
slice:
  id: slice.evidence_visible_to_teacher
  journey: journey.evidence_to_teacher_action
  outcome: outcome.teacher_turns_evidence_into_action
  status: selected
  maturity: planning
goal: >
  Teacher can view a student evidence artifact with source metadata.
starts_with:
  - evidence artifact exists
  - teacher has a relationship to the student
  - source metadata exists
ends_with:
  - teacher can open the artifact
  - source and timestamp are visible
  - unauthorized teacher cannot access it
  - stale or missing evidence is handled clearly
vertical_path:
  experience:
    - teacher evidence inbox
    - evidence detail view
  data:
    - EvidenceArtifact
    - EvidenceSource
    - StudentContext
    - TeacherAccessPolicy
  policy:
    - teacher can only view evidence for assigned students
    - source metadata must be visible
    - stale data must be marked
  tests:
    - permitted teacher can view artifact
    - unauthorized teacher cannot view artifact
    - missing artifact shows safe empty state
    - stale artifact shows freshness warning
How this interacts with BMAD:
Slice Layer
  → provides the bounded scope for:
      bmad-product-brief
      bmad-create-prd
      bmad-create-ux-design
      bmad-create-architecture
      bmad-create-epics-and-stories
This prevents BMAD from trying to plan the whole huge system at once.
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 74/88
9. Layer 6 — Capability / Landscape Layer
Capabilities are durable system abilities discovered from slices and journeys.
They are not guessed up front. They emerge.
Examples:
evidence artifact model
source metadata
role-based visibility
teacher workspace
student goal model
guidance generation
teacher decision capture
coach visibility policy
leadership aggregation
Storage:
_bmad-output/lens/landscape/
├── systems/
│   └── northstar-like-platform/
│       ├── system-ledger.md
│       └── system.yaml
├── domains/
│   ├── evidence-and-portfolios/
│   │   ├── domain-ledger.md
│   │   └── domain.yaml
│   └── teacher-workspace/
│       ├── domain-ledger.md
│       └── domain.yaml
├── capabilities/
│   ├── evidence-artifact-model/
│   │   ├── capability-ledger.md
│   │   └── capability.yaml
│   └── role-based-visibility/
│       ├── capability-ledger.md
│       └── capability.yaml
└── services/
└── evidence-service/
├── service-ledger.md
└── service.yaml
Example command:
Bash
lens-map-capabilities --slice slice.evidence_visible_to_teacher
lens-promote-capabilities --from-slice slice.evidence_visible_to_teacher
Output:
YAML
# _bmad-output/lens/landscape/capabilities/evidence-artifact-model/capability.yaml
capability:
id: capability.evidence_artifact_model
name: Evidence Artifact Model
status: active
maturity: hypothesis
purpose: >
Represent learning evidence in a durable, source-traceable way.
discovered_from:- slice.evidence_visible_to_teacher
participates_in:
outcomes:- outcome.teacher_turns_evidence_into_action
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
75/88
journeys:- journey.evidence_to_teacher_action
slices:- slice.evidence_visible_to_teacher- slice.evidence_interpreted_for_teacher- slice.evidence_aligned_to_goal
open_questions:- What counts as evidence?- Can evidence be imported?- Can evidence be manually created?- Is evidence attached to a student, goal, standard, class, or artifact collection?
related_capabilities:- capability.evidence_source_metadata- capability.role_based_visibility- capability.teacher_workspace
This is where the screenshot idea of living landscape ledgers becomes essential. A feature/slice archive
records what happened. A capability or domain ledger records the current durable truth.
10. Layer 7 — Derived Map Layer
The Derived Map is the machine-readable graph.
It is generated from source files.
It is not edited by hand.
Storage:
_bmad-output/lens/graph/
├── derived-map.json
├── derived-map.yaml
├── relationship-index.yaml
├── traceability-index.yaml
├── freshness-index.yaml
└── warnings.yaml
Example command:
Bash
lens-map-rebuild
lens-doctor
Output:
YAML
# _bmad-output/lens/graph/relationship-index.yaml
relationships:- from: outcome.teacher_turns_evidence_into_action
type: realized_by
to: journey.evidence_to_teacher_action- from: journey.evidence_to_teacher_action
type: decomposed_into
to: slice.evidence_visible_to_teacher- from: slice.evidence_visible_to_teacher
type: requires
to: capability.evidence_artifact_model- from: slice.evidence_visible_to_teacher
type: requires
to: capability.role_based_visibility
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
76/88
- from: capability.evidence_artifact_model
type: participates_in
to: domain.evidence_and_portfolios- from: capability.role_based_visibility
type: participates_in
to: domain.identity_and_access- from: slice.evidence_visible_to_teacher
type: prepared_bmad_packet
to: artifact.bmad_packet.evidence_visible_to_teacher- from: artifact.bmad.prd.evidence_visible_to_teacher
type: implements_scope_for
to: slice.evidence_visible_to_teacher
Doctor report:
YAML
# _bmad-output/lens/graph/warnings.yaml
warnings:- id: warning.orphan_capability
severity: medium
message: capability.teacher_workspace exists but is not assigned to a domain ledger.- id: warning.unresolved_decision
severity: high
message: slice.evidence_visible_to_teacher requires decision.teacher_access_policy before BMAD 
implementation.- id: warning.stale_landscape
severity: medium
message: domain.evidence_and_portfolios ledger has not been updated since two slices changed 
related capability metadata.
This layer lets agents navigate the system without forcing humans to maintain a giant graph manually.
11. Layer 8 — BMAD Bridge Layer
This layer packages LENS artifacts into BMAD-consumable context.
BMAD already has project-context.md, which its docs describe as an implementation guide for AI agents.
The docs state that implementation workflows automatically load project-context.md if it exists,
including architecture, story creation, dev story, code review, sprint planning, retrospective, and correct
course workflows. BMAD Method
LENS should generate two kinds of BMAD bridge artifacts:
1. Project-wide context
2. Slice-specific BMAD packet
Storage:
_bmad-output/
├── project-context.md
├── lens/
│   └── bmad-bridge/
│       ├── system-context.md
│       ├── active-slice-context.md
│       ├── prd-input.md
│       ├── ux-input.md
│       ├── architecture-input.md
│       ├── epic-story-input.md
│       └── readiness-check-input.md
└── planning-artifacts/
├
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
77/88
├── PRD.md
├── architecture.md
└── epics/
Example command:
Bash
lens-prepare-bmad --slice slice.evidence_visible_to_teacher
Output:
Markdown
# _bmad-output/lens/bmad-bridge/active-slice-context.md
# LENS Active Slice Context
## System
NorthStar-like Platform
## Active Outcome
Teacher can convert student evidence into a next instructional action.
## Active Journey
Evidence to Teacher Action
## Active Slice
Evidence Visible to Teacher
## Slice Goal
Teacher can view a student evidence artifact with source metadata.
## Required Capabilities- Evidence Artifact Model- Evidence Source Metadata- Teacher/Student Relationship- Role-Based Visibility- Teacher Workspace- Missing/Stale Evidence Handling
## Required Decisions Before Implementation- Minimum evidence shape- Teacher access policy- Source freshness display- Missing artifact behavior
## Acceptance Evidence- Permitted teacher can view artifact.- Unauthorized teacher cannot view artifact.- Evidence source is visible.- Evidence timestamp is visible.- Missing artifact renders safe empty state.- Stale artifact is visibly marked.
## Do Not Expand Scope Into- AI interpretation- standards alignment- coaching rollups- leadership dashboards- family visibility
Project context update:
Markdown
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
78/88
# _bmad-output/project-context.md
# Project Context for AI Agents
## LENS System Context
This project uses the LENS module.
All BMAD workflows must preserve LENS traceability:
system → outcome → journey → slice → capability → story → implementation evidence
The current active LENS context is located at:
_bmad-output/lens/bmad-bridge/active-slice-context.md
## Implementation Rule
Do not create implementation stories that are not traceable to an active LENS slice.
## Architecture Rule
Architecture decisions must update the relevant LENS capability, domain, or service ledger.
## Change Rule
If implementation reveals that an upstream assumption is wrong, raise a Salmon signal instead of 
silently changing scope.
How this interacts with BMAD:
Bash
bmad-product-brief
bmad-create-prd
bmad-create-ux-design
bmad-create-architecture
bmad-create-epics-and-stories
bmad-check-implementation-readiness
BMAD’s docs say architecture is created in Solutioning, and epics/stories are created after architecture
so technical decisions can shape the story breakdown. BMAD Method LENS should respect that: LENS
proposes slices and capabilities, but BMAD architecture determines final implementation shape.
12. Layer 9 — Implementation Guard Layer
This layer makes sure BMAD stories remain connected to LENS.
BMAD handles the implementation cycle. LENS guards traceability and coherence.
BMAD implementation flow:
bmad-sprint-planning
bmad-create-story
bmad-dev-story
bmad-code-review
bmad-retrospective
BMAD’s docs say bmad-sprint-planning creates sprint-status.yaml, and the build cycle repeats story
creation, implementation, and code review. BMAD Method
LENS wraps that with:
Bash
lens-guard-story --story story.evidence-detail-view
lens-validate-slice --slice slice.evidence_visible_to_teacher
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
79/88
Storage:
_bmad-output/lens/implementation/
├── story-traceability/
│   ├── story-evidence-detail-view.yaml
│   └── story-access-policy-test.yaml
├── validation/
│   ├── slice-validation-evidence-visible-to-teacher.md
│   └── journey-validation-evidence-to-teacher-action.md
└── salmon-signals/
Example story guard:
YAML
# _bmad-output/lens/implementation/story-traceability/story-evidence-detail-view.yaml
story:
id: story.evidence_detail_view
bmad_story_file: _bmad-output/planning-artifacts/epics/epic-001/story-evidence-detail-view.md
lens_trace:
system: system.northstar_like_platform
outcome: outcome.teacher_turns_evidence_into_action
journey: journey.evidence_to_teacher_action
slice: slice.evidence_visible_to_teacher
capabilities:- capability.evidence_artifact_model- capability.evidence_source_metadata- capability.teacher_workspace
guard_result:
status: pass
checks:- name: story_traces_to_active_slice
status: pass- name: story_does_not_expand_scope
status: pass- name: acceptance_evidence_present
status: pass- name: privacy_boundary_acknowledged
status: pass
13. Layer 10 — Salmon / Correction Layer
This is the upstream-change mechanism.
BMAD already has bmad-correct-course for significant mid-implementation changes. BMAD Method LENS
Salmon should decide when a discovery is local, when it updates the living landscape, and when it
should trigger BMAD correct-course.
Salmon signal flow:
Story discovers issue
↓
Slice affected?
↓
Journey affected?
↓
Outcome affected?
↓
Capability/domain/service ledger affected?
↓
BMAD plan affected?
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
80/88
↓
Run bmad-correct-course if needed
Storage:
_bmad-output/lens/salmon/
├── signals/
│   ├── salmon-001.yaml
│   └── salmon-002.yaml
├── propagation/
│   └── salmon-001-propagation.md
└── decisions/
└── salmon-001-decision.md
Example:
YAML
# _bmad-output/lens/salmon/signals/salmon-001.yaml
salmon_signal:
id: salmon.001
raised_from: story.evidence_detail_view
severity: high
discovery: >
Teacher access cannot be determined from class roster alone.
Some support staff need visibility without being the assigned classroom teacher.
impacted_nodes:- slice.evidence_visible_to_teacher- capability.teacher_student_relationship- capability.role_based_visibility- decision.teacher_access_policy- domain.identity_and_access
recommended_action:
type: correct_course
reason: >
The slice acceptance criteria and architecture assumptions depend on
an incomplete access model.
bmad_action:
suggested_workflow: bmad-correct-course
How this interacts with BMAD:
Bash
lens-salmon raise --from-story story.evidence_detail_view
lens-salmon propagate --signal salmon.001
bmad-correct-course
lens-sync-bmad --after-correct-course
This makes LENS the system-coherence detector and BMAD the formal correction workflow.
14. Layer 11 — Auspex Visibility Layer
Auspex is the stakeholder visibility layer.
It should not be the source of truth.
It should read the Derived Map and show:
system status
active outcomes
active journeys
active slices
artifact freshness
open decisions
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
81/88
risks
blockers
BMAD progress
validation evidence
Salmon signals
Storage:
_bmad-output/lens/auspex/
├── status.json
├── dashboard.html
├── stakeholder-summary.md
└── read-only-report.md
Command:
Bash
lens-auspex build
lens-auspex serve
Example status:
YAML
# _bmad-output/lens/auspex/status.yaml
system: system.northstar_like_platform
active_focus:
outcome: outcome.teacher_turns_evidence_into_action
journey: journey.evidence_to_teacher_action
slice: slice.evidence_visible_to_teacher
bmad_status:
product_brief: complete
prd: in_progress
ux_design: not_started
architecture: not_started
epics_and_stories: not_started
implementation_readiness: not_started
open_decisions:- decision.minimum_evidence_shape- decision.teacher_access_policy- decision.source_freshness_display
risks:- risk.privacy_boundary- risk.evidence_definition_ambiguity
salmon_signals:
open: 0
resolved: 0
artifact_freshness:
system_thesis: fresh
outcome_matrix: fresh
journey_map: fresh
slice_context: fresh
capability_ledgers: needs_review
15. How relationships are discovered
LENS discovers relationships in stages.
Stage 1: Extracted relationship
Stage 2: Hypothesized relationship
Stage 3: Human-reviewed relationship
Stage 4: Promoted landscape relationship
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
82/88
Stage 5: Implementation-validated relationship
Stage 6: Derived map relationship
Example:
YAML
# Relationship lifecycle
relationship:
id: rel.001
from: outcome.teacher_turns_evidence_into_action
type: realized_by
to: journey.evidence_to_teacher_action
status: hypothesized
confidence: medium
discovered_from:- extraction.001
review:
status: human_review_needed
promotion:
landscape_status: not_promoted
validation:
implementation_status: not_validated
After review:
YAML
relationship:
id: rel.001
from: outcome.teacher_turns_evidence_into_action
type: realized_by
to: journey.evidence_to_teacher_action
status: active
confidence: high
review:
status: approved
promotion:
landscape_status: promoted
validation:
implementation_status: partially_validated
validated_by:- slice.evidence_visible_to_teacher
Relationship types:
YAML
relationship_types:- id: expresses
example: system_thesis expresses system intent- id: serves
example: outcome serves role- id: realized_by
example: outcome realized_by journey- id: decomposed_into
example: journey decomposed_into slices- id: requires
example: slice requires capability- id: participates_in
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
83/88
example: capability participates_in domain- id: implemented_by
example: capability implemented_by service- id: planned_by
example: slice planned_by BMAD PRD- id: decomposed_by
example: PRD decomposed_by BMAD epics- id: implemented_by_story
example: slice implemented_by story- id: validated_by
example: acceptance evidence validates slice- id: impacted_by
example: domain impacted_by Salmon signal- id: supersedes
example: new decision supersedes old assumption
16. How relationships are worked through
Relationships are worked through by gates.
Discovery gate:
Do we have enough signal to model this relationship?
Challenge gate:
Is this relationship coherent, or is it an AI guess?
Promotion gate:
Should this relationship update the living landscape?
BMAD gate:
Should this relationship constrain PRD, UX, architecture, or stories?
Implementation gate:
Did the code prove or disprove the relationship?
Salmon gate:
Did downstream work reveal upstream truth?
Validation gate:
Did the relationship help satisfy an outcome?
Example:
Candidate relationship:
Teacher outcome → Evidence-to-action journey
Discovery:
Extracted from brainstorm.
Challenge:
Could this actually be two journeys?
1. Teacher reviews evidence.
2. Teacher selects instructional action.
Promotion:
Approved as one journey with multiple slices.
BMAD:
Used to generate PRD scope and UX flow.
Implementation:
First slice proves evidence visibility only.
Salmon:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
84/88
Implementation reveals teacher access policy is more complex than expected.
Correction:
Update role-based visibility capability.
Possibly run bmad-correct-course.
Validation:
Outcome not fully satisfied yet, but journey step 1 is validated.
17. End-user flow inside BMAD
The user experience should feel like BMAD with an added LENS track.
Bash
# Install BMAD and LENS module
npx bmad-method install \--modules bmm \--custom-source ./lens-module \--tools claude-code \--yes
# Ask BMAD what to do
bmad-help
Since BMAD Help detects installed modules and recommends next actions, LENS should register itself so
bmad-help can suggest LENS workflows for large ambiguous systems. BMAD’s docs describe BMad-Help
as inspecting the project, detecting installed modules, and recommending next steps. BMAD Method
User says:
bmad-help I have a large ambiguous product idea with many roles and workflows.
BMAD should recommend:
This looks like a large complex system.
Use the LENS discovery track before creating a PRD.
Recommended:
1. lens-discover start
2. lens-synthesize
3. lens-map-system
4. lens-map-outcomes
5. lens-map-journeys
6. lens-slice-journey
7. lens-prepare-bmad
8. bmad-product-brief
9. bmad-create-prd
Example command flow:
Bash
lens-discover start --system learning-improvement-platform --mode brainstorm
lens-synthesize --system learning-improvement-platform
lens-map-system --system learning-improvement-platform
lens-map-outcomes --system learning-improvement-platform
lens-map-journeys --outcome outcome.teacher_turns_evidence_into_action
lens-slice-journey journey.evidence_to_teacher_action
lens-focus-slice slice.evidence_visible_to_teacher
lens-map-capabilities --slice slice.evidence_visible_to_teacher
lens-analyze-impact --slice slice.evidence_visible_to_teacher
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
85/88
lens-prepare-bmad --slice slice.evidence_visible_to_teacher
Then BMAD takes over:
Bash
bmad-product-brief
bmad-create-prd
bmad-create-ux-design
bmad-create-architecture
bmad-create-epics-and-stories
bmad-check-implementation-readiness
bmad-sprint-planning
Then LENS guards implementation:
Bash
bmad-create-story
lens-guard-story --latest
bmad-dev-story
lens-validate-slice --slice slice.evidence_visible_to_teacher
bmad-code-review
lens-sync-bmad --slice slice.evidence_visible_to_teacher
lens-map-rebuild
lens-doctor
18. Example: digesting a NorthStar-scale idea without a website
Input from user:
I want to build a platform for schools where students, teachers, coaches,
and leaders all work from the same learning improvement system. It should use
evidence from student work, help teachers know what to do next, help coaches
support teachers, and help leaders see where the school needs support.
LENS discovery output:
YAML
system:
id: system.learning_improvement_platform
status: hypothesis
candidate_roles:- role.student- role.teacher- role.coach- role.school_leader- role.district_leader
candidate_operating_loop:
id: loop.learning_improvement
steps:- assess- set_goals- learn- apply- evidence- improve
candidate_outcomes:- id: outcome.student_understands_next_goal- id: outcome.teacher_turns_evidence_into_action- id: outcome.coach_identifies_support_need- id: outcome.leader_sees_implementation_health
candidate_risks:
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
86/88
  - risk.ai_overreach
  - risk.student_privacy
  - risk.teacher_surveillance
  - risk.dashboard_sprawl
LENS focus:
YAML
focused_outcome:
  id: outcome.teacher_turns_evidence_into_action
  reason: >
    This outcome connects student work, teacher action, evidence modeling,
    AI guidance, and future coaching loops without requiring the entire
    platform to exist first.
LENS journey:
YAML
journey:
  id: journey.evidence_to_teacher_action
steps:
  - evidence captured
  - teacher views evidence
  - system interprets evidence
  - evidence aligns to goal or standard
  - system suggests next action
  - teacher accepts, adjusts, or rejects
  - decision is recorded
  - coaching signal may be created
LENS slice roadmap:
YAML
slices:
  - id: slice.evidence_visible_to_teacher
    scope: evidence artifact visible with source metadata
  - id: slice.evidence_interpreted_for_teacher
    scope: plain-language interpretation visible
  - id: slice.evidence_aligned_to_goal
    scope: artifact linked to goal or standard
  - id: slice.guidance_recommended_to_teacher
    scope: explainable next action recommendation
  - id: slice.teacher_decision_recorded
    scope: accept/adjust/reject captured
  - id: slice.coach_signal_created
    scope: support signal created without inappropriate student-level exposure
LENS prepares first BMAD packet:
YAML
bmad_packet:
  active_slice: slice.evidence_visible_to_teacher
  include:
    - system_thesis
    - focused_outcome
    - journey_context
    - slice_scope
    - required_capabilities
    - risks
    - decisions_needed
    - acceptance_evidence
  exclude:
    - AI recommendations
Printed using ChatGPT to PDF, powered by PDFCrowd HTML to PDF API. 87/88
- coaching dashboards- district analytics- family portal
BMAD then creates:
_bmad-output/planning-artifacts/
├── PRD.md
├── architecture.md
└── epics/
├── epic-001-evidence-visibility.md
├── story-001-evidence-artifact-model.md
├── story-002-teacher-evidence-inbox.md
├── story-003-evidence-detail-view.md
└── story-004-access-policy-tests.md
LENS syncs back:
YAML
# _bmad-output/lens/graph/traceability-index.yaml
traceability:
slice.evidence_visible_to_teacher:
bmad_prd: _bmad-output/planning-artifacts/PRD.md
bmad_architecture: _bmad-output/planning-artifacts/architecture.md
stories:- story.evidence_artifact_model- story.teacher_evidence_inbox- story.evidence_detail_view- story.access_policy_tests
capabilities_validated:- capability.evidence_artifact_model- capability.evidence_source_metadata- capability.role_based_visibility
outcome_progress:
outcome.teacher_turns_evidence_into_action: partially_validated
19. The clean final mental model
BMAD without LENS:
Idea → PRD → Architecture → Epics → Stories → Code
BMAD with LENS:
Messy idea
→ Capture
→ Extraction
→ System thesis
→ Role/outcome map
→ Journey graph
→ Vertical slices
→ Capability landscape
→ Derived relationship map
→ BMAD packet
→ PRD
→ Architecture
→ Epics/stories
→ Implementation
→ Validation
→ Landscape update
→ Salmon correction when reality disagrees
Or even shorter:
LENS makes the system understandable.
BMAD makes the system buildable.
LENS checks whether the built work still matches the system.
Printed using 
ChatGPT to PDF, powered by PDFCrowd 
HTML to PDF API.
88/88