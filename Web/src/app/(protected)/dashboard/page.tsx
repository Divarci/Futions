import React from "react";

export default function DashboardPage() {
	const cards = [
		{ id: "a", title: "Tasks", value: 8 },
		{ id: "b", title: "Projects", value: 3 },
		{ id: "c", title: "Messages", value: 12 },
	];

	return (
		<main className="p-6 max-w-5xl mx-auto">
			<h1 className="text-2xl font-semibold mb-4">Dashboard — Overview (Placeholder)</h1>

			<section className="grid grid-cols-1 sm:grid-cols-3 gap-4 mb-6">
				{cards.map((c) => (
					<div key={c.id} className="bg-background border-border p-4 rounded-lg text-center">
						<div className="text-muted-fg">{c.title}</div>
						<div className="text-2xl font-bold text-foreground">{c.value}</div>
					</div>
				))}
			</section>

			<div className="bg-muted border-border p-4 rounded-lg">
				<h2 className="text-lg font-medium mb-2">Activity Feed</h2>
				<ul className="space-y-2 text-sm text-muted-fg">
					<li>• Sample activity A (placeholder)</li>
					<li>• Sample activity B (placeholder)</li>
					<li>• Sample activity C (placeholder)</li>
				</ul>
			</div>
		</main>
	);
}
