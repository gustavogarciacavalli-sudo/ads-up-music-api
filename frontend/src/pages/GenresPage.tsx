import { useEffect, useState } from 'react';
import { Genre } from '../types/Genre';
import { genreApi } from '../api/api';
import Loading from '../components/Loading';
import ErrorMessage from '../components/ErrorMessage';

export default function GenresPage() {
  const [genres, setGenres] = useState<Genre[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [formError, setFormError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [editId, setEditId] = useState<number | null>(null);
  const [form, setForm] = useState({ name: '', description: '' });

  useEffect(() => { loadGenres(); }, []);

  async function loadGenres() {
    try {
      setLoading(true); setError('');
      const data = await genreApi.getAll();
      setGenres(data);
    } catch { setError('Erro ao carregar gêneros. Verifique se o backend está rodando.'); }
    finally { setLoading(false); }
  }

  function openCreate() {
    setForm({ name: '', description: '' });
    setEditId(null); setFormError(''); setShowForm(true);
  }

  function openEdit(genre: Genre) {
    setForm({ name: genre.name, description: genre.description });
    setEditId(genre.id); setFormError(''); setShowForm(true);
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setFormError('');
    if (!form.name.trim()) { setFormError('Nome é obrigatório.'); return; }
    try {
      if (editId) {
        await genreApi.update(editId, form);
      } else {
        await genreApi.create(form);
      }
      setShowForm(false);
      loadGenres();
    } catch (err: any) { setFormError(err.message || 'Erro ao salvar gênero.'); }
  }

  async function handleDelete(id: number) {
    if (!confirm('Deseja realmente excluir este gênero?')) return;
    try {
      await genreApi.delete(id);
      loadGenres();
    } catch (err: any) { setError(err.message || 'Erro ao excluir gênero.'); }
  }

  if (loading) return <Loading />;
  if (error) return <ErrorMessage message={error} />;

  return (
    <div className="page">
      <div className="page-header">
        <h1 className="page-title">🎸 Gêneros Musicais</h1>
        <button className="btn btn-primary" onClick={openCreate}>+ Novo Gênero</button>
      </div>

      {showForm && (
        <div className="modal-overlay" onClick={() => setShowForm(false)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>{editId ? 'Editar Gênero' : 'Novo Gênero'}</h2>
            {formError && <div className="form-error">{formError}</div>}
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Nome *</label>
                <input type="text" value={form.name} onChange={e => setForm({...form, name: e.target.value})} placeholder="Nome do gênero" />
              </div>
              <div className="form-group">
                <label>Descrição</label>
                <textarea value={form.description} onChange={e => setForm({...form, description: e.target.value})} placeholder="Descrição do gênero" />
              </div>
              <div className="form-actions">
                <button type="submit" className="btn btn-primary">{editId ? 'Salvar' : 'Criar'}</button>
                <button type="button" className="btn btn-secondary" onClick={() => setShowForm(false)}>Cancelar</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {genres.length === 0 ? (
        <p className="empty-state">Nenhum gênero cadastrado. Clique em "+ Novo Gênero" para começar.</p>
      ) : (
        <div className="table-container">
          <table className="data-table">
            <thead>
              <tr><th>ID</th><th>Nome</th><th>Descrição</th><th>Ações</th></tr>
            </thead>
            <tbody>
              {genres.map(g => (
                <tr key={g.id}>
                  <td>{g.id}</td>
                  <td>{g.name}</td>
                  <td>{g.description}</td>
                  <td className="actions-cell">
                    <button className="btn btn-sm btn-edit" onClick={() => openEdit(g)}>✏️ Editar</button>
                    <button className="btn btn-sm btn-delete" onClick={() => handleDelete(g.id)}>🗑️ Excluir</button>
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
