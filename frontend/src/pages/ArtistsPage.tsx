import { useEffect, useState } from 'react';
import { Artist } from '../types/Artist';
import { Genre } from '../types/Genre';
import { artistApi, genreApi } from '../api/api';
import Loading from '../components/Loading';
import ErrorMessage from '../components/ErrorMessage';

export default function ArtistsPage() {
  const [artists, setArtists] = useState<Artist[]>([]);
  const [genres, setGenres] = useState<Genre[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [formError, setFormError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [editId, setEditId] = useState<number | null>(null);
  const [form, setForm] = useState({ name: '', bio: '', genreId: 0 });

  useEffect(() => { loadData(); }, []);

  async function loadData() {
    try {
      setLoading(true); setError('');
      const [a, g] = await Promise.all([artistApi.getAll(), genreApi.getAll()]);
      setArtists(a); setGenres(g);
    } catch { setError('Erro ao carregar artistas.'); }
    finally { setLoading(false); }
  }

  function openCreate() {
    setForm({ name: '', bio: '', genreId: genres.length > 0 ? genres[0].id : 0 });
    setEditId(null); setFormError(''); setShowForm(true);
  }

  function openEdit(artist: Artist) {
    setForm({ name: artist.name, bio: artist.bio, genreId: artist.genreId });
    setEditId(artist.id); setFormError(''); setShowForm(true);
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault(); setFormError('');
    if (!form.name.trim()) { setFormError('Nome é obrigatório.'); return; }
    if (!form.genreId) { setFormError('Selecione um gênero.'); return; }
    try {
      if (editId) { await artistApi.update(editId, form); }
      else { await artistApi.create(form); }
      setShowForm(false); loadData();
    } catch (err: any) { setFormError(err.message || 'Erro ao salvar artista.'); }
  }

  async function handleDelete(id: number) {
    if (!confirm('Deseja realmente excluir este artista?')) return;
    try { await artistApi.delete(id); loadData(); }
    catch (err: any) { setError(err.message || 'Erro ao excluir artista.'); }
  }

  if (loading) return <Loading />;
  if (error) return <ErrorMessage message={error} />;

  return (
    <div className="page">
      <div className="page-header">
        <h1 className="page-title">🎤 Artistas</h1>
        <button className="btn btn-primary" onClick={openCreate}>+ Novo Artista</button>
      </div>

      {showForm && (
        <div className="modal-overlay" onClick={() => setShowForm(false)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>{editId ? 'Editar Artista' : 'Novo Artista'}</h2>
            {formError && <div className="form-error">{formError}</div>}
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Nome *</label>
                <input type="text" value={form.name} onChange={e => setForm({...form, name: e.target.value})} placeholder="Nome do artista" />
              </div>
              <div className="form-group">
                <label>Bio</label>
                <textarea value={form.bio} onChange={e => setForm({...form, bio: e.target.value})} placeholder="Biografia do artista" />
              </div>
              <div className="form-group">
                <label>Gênero *</label>
                <select value={form.genreId} onChange={e => setForm({...form, genreId: Number(e.target.value)})}>
                  <option value={0}>Selecione um gênero</option>
                  {genres.map(g => <option key={g.id} value={g.id}>{g.name}</option>)}
                </select>
              </div>
              <div className="form-actions">
                <button type="submit" className="btn btn-primary">{editId ? 'Salvar' : 'Criar'}</button>
                <button type="button" className="btn btn-secondary" onClick={() => setShowForm(false)}>Cancelar</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {artists.length === 0 ? (
        <p className="empty-state">Nenhum artista cadastrado.</p>
      ) : (
        <div className="table-container">
          <table className="data-table">
            <thead>
              <tr><th>ID</th><th>Nome</th><th>Bio</th><th>Gênero</th><th>Ações</th></tr>
            </thead>
            <tbody>
              {artists.map(a => (
                <tr key={a.id}>
                  <td>{a.id}</td>
                  <td>{a.name}</td>
                  <td>{a.bio}</td>
                  <td><span className="badge">{a.genreName}</span></td>
                  <td className="actions-cell">
                    <button className="btn btn-sm btn-edit" onClick={() => openEdit(a)}>✏️ Editar</button>
                    <button className="btn btn-sm btn-delete" onClick={() => handleDelete(a.id)}>🗑️ Excluir</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
