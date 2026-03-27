import React from "react";

export default function HomePage() {
	const items = [
		{ id: "1", title: "Sample Task A", description: "Temporary placeholder item." },
		{ id: "2", title: "Sample Task B", description: "Temporary placeholder item." },
		{ id: "3", title: "Sample Task C", description: "Temporary placeholder item." },
	];

	return (
		<main className="p-6 max-w-5xl mx-auto">
			<div className="flex items-center justify-between mb-6">
				<h1 className="text-2xl font-semibold">Futions — Dashboard (Placeholder)</h1>
				<div className="text-sm text-muted-fg">Temporary data for development</div>
			</div>

			<section className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
				<div className="md:col-span-2 bg-background border-border p-4 rounded-lg shadow-sm">
					<h2 className="text-lg font-medium mb-3">Recent Items</h2>
					<ul className="space-y-3">
						{items.map((it) => (
							<li key={it.id} className="p-3 bg-muted rounded-md border-border">
								<div className="flex items-center justify-between">
									<strong>{it.title}</strong>
									<span className="text-xs text-muted-fg">#{it.id}</span>
								</div>
								<p className="mt-1 text-sm text-muted-fg">{it.description}</p>
							</li>
						))}
					</ul>
				</div>

				<div className="bg-background border-border p-4 rounded-lg shadow-sm">
					<h2 className="text-lg font-medium mb-3">Quick Stats</h2>
					<div className="space-y-2 text-sm text-muted-fg">
						<div className="flex justify-between"><span>Active users</span><strong className="text-foreground">24</strong></div>
						<div className="flex justify-between"><span>Open tasks</span><strong className="text-foreground">8</strong></div>
						<div className="flex justify-between"><span>Errors (sample)</span><strong className="text-foreground">0</strong></div>
					</div>
				</div>
			</section>

			<footer className="text-center text-sm text-muted-fg">This page contains temporary placeholder UI. Replace with real components later.</footer>
		</main>
	);
}
